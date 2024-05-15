from langchain_community.llms import HuggingFaceEndpoint
import os

HF_API_TOKEN = os.environ["HF_API_TOKEN"]

from langchain_community.document_loaders import TextLoader
from langchain_openai import OpenAIEmbeddings
from langchain_text_splitters import CharacterTextSplitter
from langchain_chroma import Chroma

# Load the document, split it into chunks, embed each chunk and load it into the vector store.
raw_documents = TextLoader('./example_email.txt').load()
text_splitter = CharacterTextSplitter(chunk_size=1000, chunk_overlap=0)
documents = text_splitter.split_documents(raw_documents)

from langchain.vectorstores import Chroma

vectorstore = Chroma.from_documents(documents=documents, embedding=OpenAIEmbeddings())


llm = HuggingFaceEndpoint(repo_id="mistralai/Mixtral-8x7B-Instruct-v0.1",
                    temperature=0.1, 
                    model_kwargs={"max_length": 100},
                    huggingfacehub_api_token=HF_API_TOKEN)

retriever=vectorstore.as_retriever()

from langchain.chains import create_retrieval_chain
from langchain.chains.combine_documents import create_stuff_documents_chain
from langchain_core.prompts import ChatPromptTemplate

system_prompt = (
    "Use the given context to answer the question. "
    "If you don't know the answer, say you don't know. "
    "Use three sentence maximum and keep the answer concise. "
    "Context: {context}"
)
prompt = ChatPromptTemplate.from_messages(
    [
        ("system", system_prompt),
        ("human", "{input}"),
    ]
)
question_answer_chain = create_stuff_documents_chain(llm, prompt)
chain = create_retrieval_chain(retriever, question_answer_chain)

print(chain.invoke({"input": "Summarise the email in 3 bullet points."})['answer'])
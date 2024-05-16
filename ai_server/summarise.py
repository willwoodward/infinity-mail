from langchain_community.llms import HuggingFaceEndpoint
import os
from langchain_chroma import Chroma
from langchain.chains import create_retrieval_chain
from langchain.chains.combine_documents import create_stuff_documents_chain
from langchain_core.prompts import ChatPromptTemplate
from langchain_community.embeddings.sentence_transformer import (
    SentenceTransformerEmbeddings,
)
from vector_store import VectorStore

print("Imported")
HF_API_TOKEN = os.environ["HF_API_TOKEN"]

db = VectorStore()
db.add_raw('./example_email.txt')
retriever=db.retriever

llm = HuggingFaceEndpoint(repo_id="mistralai/Mixtral-8x7B-Instruct-v0.1",
                    temperature=0.1, 
                    model_kwargs={"max_length": 100},
                    huggingfacehub_api_token=HF_API_TOKEN)

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
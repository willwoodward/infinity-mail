from langchain_community.llms import HuggingFaceEndpoint
from langchain.chains import create_retrieval_chain
from langchain.chains.combine_documents import create_stuff_documents_chain
from langchain_core.prompts import ChatPromptTemplate
import os

class LLM:
    def __init__(self, retriever=None):
        self.__llm = HuggingFaceEndpoint(repo_id="mistralai/Mixtral-8x7B-Instruct-v0.1",
                    temperature=0.1, 
                    model_kwargs={"max_length": 100},
                    huggingfacehub_api_token=os.environ["HF_API_TOKEN"])
        
        self.__retriever = retriever
        return

    def doc_question(self, user_prompt):
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
        
        question_answer_chain = create_stuff_documents_chain(self.__llm, prompt)
        chain = create_retrieval_chain(self.__retriever, question_answer_chain)

        return chain.invoke({"input": user_prompt})['answer']
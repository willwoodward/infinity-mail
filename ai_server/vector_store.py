from langchain_chroma import Chroma
from langchain_community.embeddings.sentence_transformer import SentenceTransformerEmbeddings
from langchain_community.document_loaders import TextLoader
from langchain_text_splitters import CharacterTextSplitter
from langchain.schema.document import Document

class VectorStore:
    def __init__(self):
        # Initialise vector store (either on disk or when the class starts up)
        # Initialise the retriever
        # Initialise embedding function
        embedding_function = SentenceTransformerEmbeddings(model_name="all-MiniLM-L6-v2")
        
        self.__db = Chroma(embedding_function=embedding_function)
        self.retriever = self.__db.as_retriever()

    def add(self, docs):
        # Store the documents in the vector store
        self.__db.add_documents(documents=docs)
        return
    
    def add_raw(self, raw_docs):
        text_splitter = CharacterTextSplitter(chunk_size=1000, chunk_overlap=0)
        documents = [Document(page_content=x) for x in text_splitter.split_text(raw_docs)]
        
        # Store the raw documents in the vector store
        self.add(documents)
        return

store = VectorStore()
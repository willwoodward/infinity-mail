from vector_store import VectorStore
from llm import LLM
print("Imported")

db = VectorStore()
db.add_raw('./example_email.txt')

llm = LLM(db.retriever)
ans = llm.doc_question("Summarise the email in 3 bullet points.")
print(ans)

ans = llm.doc_question("Who is the recipient of this email?")
print(ans)
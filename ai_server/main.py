from flask import Flask
from flask import request
from vector_store import VectorStore
from llm import LLM

app = Flask(__name__)

db = VectorStore()
llm = LLM(db.retriever)

@app.route("/")
def hello_world():
    return "<p>Hello, World!</p>"

@app.route('/add_doc', methods = ['POST'])
def update_db():
    data = request.json
    db.add_raw(data['document'])
    return "Document added to database."

@app.route("/query")
def query_db():
    query = request.args.get('query')
    ans = llm.doc_question(query)
    return ans
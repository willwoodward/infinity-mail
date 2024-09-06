from flask import Flask
from flask import request
from vector_store import VectorStore
from llm import LLM

app = Flask(__name__)

db = VectorStore()
llm = LLM(db.retriever)

@app.route('/ai/add_doc', methods = ['POST'])
def update_db():
    data = request.json
    db.add_raw(data['document'])
    return "Document added to database.", 200

@app.route('/ai/switch', methods = ['POST'])
def switch_db():
    data = request.json
    db.switch(data['document'])
    return "Document added to database, and other items removed.", 200

@app.route("/ai/query")
def query_db():
    query = request.args.get('query')
    ans = llm.doc_question(query)
    return ans, 200

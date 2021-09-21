from flask import Flask
from flask import request
from flask import render_template
from inference import get_intent


app = Flask(__name__)


#print(get_intent("Turn this off"))



@app.route("/", methods=['POST'])
def index():
    if request.method == "POST":

        print("received utterance")
        #utterance = request.args.get('utterance')
        utterance = request.json['utterance']

        print(utterance)


        result = get_intent(utterance)

        return {"result": result}





if __name__ == "__main__":
    app.run(debug=True)

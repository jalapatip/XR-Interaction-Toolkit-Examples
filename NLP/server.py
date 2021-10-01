from flask import Flask
from flask import request
from flask import render_template
from inference import get_intent

app = Flask(__name__)


# print(get_intent("Turn this off"))


@app.route("/", methods=['POST'])
def index():
    if request.method == "POST":
        if 'utterance' in request.json:
            print("received utterance")

            # utterance = request.args.get('utterance')
            utterance = request.json['utterance']

            # print(utterance)
            result = get_intent(utterance)

            return {"result": result}

        elif "dynamic_position" in request.json:
            # toReturn = "\n" + timestamp + ","
            # + headPos.x + "," + headPos.y + "," + headPos.z + ","
            # + headRot.x + "," + headRot.y + "," + headRot.z + ","
            # + headRotQ.x + "," + headRotQ.y + "," + headRotQ.z + "," + headRotQ.w + ","
            # + handRPos.x + "," + handRPos.y + "," + handRPos.z + ","
            # + handRRot.x + "," + handRRot.y + "," + handRRot.z + ","
            # + handRRotQ.x + "," + handRRotQ.y + "," + handRRotQ.z + "," + handRRotQ.w + ","
            # + handLPos.x + "," + handLPos.y + "," + handLPos.z + ","
            # + handLRot.x + "," + handLRot.y + "," + handLRot.z + ","
            # + handLRotQ.x + "," + handLRotQ.y + "," + handLRotQ.z + "," + handLRotQ.w + ",";

            print('receive head/hand positions')

            # TODO post-processing for head and hand positions
            print(request.json)

            return {"result": request.json}

        else:  # device_info in request.json
            print('receive devices')

            # TODO pre-processing get gaze object list
            print(request.json)

            return {"result": request.json}

    return {"result": "wrong request; only post is accepted"}


if __name__ == "__main__":
    app.run(debug=True)

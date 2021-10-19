from flask import Flask
from flask import request
from flask import render_template
# from inference import get_intent
from rasa_nlu.training_data import load_data
from rasa_nlu.config import RasaNLUModelConfig
from rasa_nlu.model import Trainer
from rasa_nlu import config

import re
from spatial_algorithm import *

from rasa_nlu.model import Metadata, Interpreter

print("loading")
model_directory = "./trained_model/default/model_20211007-101147"
interpreter = Interpreter.load(model_directory)

# return interpreter


# class MyFlaskApp(Flask):
#   def run(self, host=None, port=None, debug=None, load_dotenv=True, **options):
#     if not self.debug or os.getenv('WERKZEUG_RUN_MAIN') == 'true':
#       with self.app_context():
#         interpreter = load_model()
#     super(MyFlaskApp, self).run(host=host, port=port, debug=debug, load_dotenv=load_dotenv, **options)


app = Flask(__name__)


# print(get_intent("Turn this off"))


@app.route("/", methods=['POST'])
def index():
    if request.method == "POST":

        if 'utterance' in request.json:
            global interpreter
            print("received utterance")

            # utterance = request.args.get('utterance')
            utterance = request.json['utterance']

            print(utterance)
            result = interpreter.parse(utterance)

            # result = get_intent(utterance)

            return {"result": result}

        elif 'device_info' in request.json and 'user_info' in request.json:  # device_info in request.json
            print('receive devices and user info')

            received_data = request.json
            all_devices = received_data['device_info']
            user_info = received_data['user_info']
            utterance = user_info['utterance']

            cosine_distance = {}
            euclidean_distance = {}
            for device in all_devices:
                # print(device)
                distance_key = f'{device["instance_id"]}-{device["appliance_type"]}'
                head_pos = [float(pos) for pos in re.split(", |\[|\]|\(|\)", user_info["headPos"]) if pos != '']
                head_rotation = [float(pos) for pos in re.split(", |\[|\]|\(|\)", user_info["headRot"]) if pos != '']
                equipment_pos = [float(pos) for pos in re.split(", |\[|\]|\(|\)", device["position"]) if pos != '']

                cosine_distance[distance_key] = get_cosine_distance(head_pos, rotation2direction(head_rotation),
                                                                    equipment_pos)
                euclidean_distance[distance_key] = get_euclidean_distance(head_pos, equipment_pos)

            processed_data = {
                "cosine_distance": cosine_distance,
                "euclidean_distance": euclidean_distance,
                "utterance": utterance,
            }

            # TODO add model inference part here

            # return pred
            return {"result": processed_data}

    return {"result": "wrong request; only post is accepted"}


if __name__ == "__main__":
    app.run(debug=True)

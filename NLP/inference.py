from rasa_nlu.training_data  import load_data
from rasa_nlu.config import RasaNLUModelConfig
from rasa_nlu.model import Trainer
from rasa_nlu import config

import warnings
warnings.filterwarnings("ignore", category=DeprecationWarning) 

# from rasa_nlu.model import Metadata, Interpreter
# model_directory = "./trained_model/default/model_20210913-154051"
# interpreter = Interpreter.load(model_directory)
# result = interpreter.parse(u"Turn this off")

# print("========================")
# print(result)


def get_intent(utterance):
	from rasa_nlu.model import Metadata, Interpreter
	model_directory = "./trained_model/default/model_20210913-154051"
	interpreter = Interpreter.load(model_directory)
	result = interpreter.parse(utterance)

	print("========================")
	print(result)
	return result


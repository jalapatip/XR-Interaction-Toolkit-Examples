from rasa_nlu.training_data  import load_data
from rasa_nlu.config import RasaNLUModelConfig
from rasa_nlu.model import Trainer
from rasa_nlu import config
import spacy
#import joblib 

train_data = load_data('rasa_dataset.json')

trainer = Trainer(config.load("config_spacy.yaml"))
#trainer = spacy.load("en_core_web_sm")

trainer.train(train_data)

model_directory = trainer.persist('./trained_model/')
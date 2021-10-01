from ramdom import seed
from ramdom import ranndint 

#data
spacial_val = {"a":0.95, "b":0.91, "c":0.9}
dis_val = {}

availability_val = {"a":1, "b":1, "c":0}
entity_val = {"a":0, "b":1, "c": 0}
alpha_vec = [0.3,0.3,0.3]
num_epochs = 10
learning_rate = 0.01

spacial_val_list = [spacial_val]
availability_val = [availability_val]
entity_val = [entity_val]
correct_answer = "a"
batch_size = 5

training_data = [[spacial_val_list, availability_val_list, entity_val_list, correct_answer]]

def get_list(training_data, batch_size):
	retrieved_entries = []
	for i in range(0, number_of_entries):
		index = randint(0, len(training_data))
		received_entries.append(training_data[i])
	return received_entries

def randomly_update_alpha(alpha_vec):
	index = randint(0,2)
	for i in range(2):
		if index == i:
			alpha_vec[i] += learning_rate
		else:
			alpha_vec[i] -= learning_rate / 2
	return alpha_vec, index



def train(learning_rate, alpha_vec, training_data):
	for i in num_epochs:
		print("epoch: "+str(i))

		#randomize which alpha value to change 
		alpha_vec, index = randomly_update_alpha(alpha_vec)

		#test new alpha vec on 5 samples from training data
		validation_set = get_list(training_data, batch_size)

		validation_success_num = validation(validation_set)

		if validation_success_num >= batch_size/2 :
			update_alpha(alpha_vec, index, 0)
		else:
			update_alpha(alpha_vec, index, 1)

		print(alpha_vec)



	



def get_compound_ranking(list_a, list_b, list_c, alpha_vec):
	curr_compound_ranking = {}
	for key, value in list_a:
		list_a_value = list_a[key]
		list_b_value = list_b[key]
		list_c_value = list_c[key]
		curr_compound_ranking[key] = list_a_value * alpha_vec[0] + list_b_value * alpha_vec[1] + list_c_value * alpha_vec[2]

	return curr_compound_ranking

def validation(validation_set):
	validation_success_num = 0
	
	for validation_data in validation_set:

		spacial_val_list = validation_data[0] 
		availability_val_list = validation_data[1]
		entity_val_list = validation_data[2]
		correct_answer = validation_data[3]

		curr_compound_ranking = get_compound_ranking(spacial_val_list, availability_val_list, entity_val_list)

		if validation_each(curr_compound_ranking, correct_answer):
			validation_success_num += 1

	return validation_success_num


def validation_each(curr_compound_ranking, correct_answer):

	curr_answer = max(curr_compound_ranking, key=curr_compound_ranking.get)
	if curr_answer == correct_answer:
		return True
	else:
		return False



def update_alpha(alpha_vec, index, validation_result):
	if validation_result == 0: #success
		for i in range(2):
			if index == i:
				alpha_vec[i] += learning_rate
			else:
				alpha_vec[i] -= learning_rate / 2

	else: #failed 
		for i in range(2):
			if index == i:
				alpha_vec[i] -= learning_rate
			else:
				alpha_vec[i] += learning_rate / 2

	return alpha_vec
		


from random import seed
from random import randint

# data
spacial_val = {"a": 0.95, "b": 0.91, "c": 0.9}
dis_val = {}

availability_val = {"a": 1, "b": 1, "c": 0}
entity_val = {"a": 0, "b": 1, "c": 0}
alpha_vec = [0.3, 0.3, 0.3]
num_epochs = 10000
learning_rate = 0.01

spacial_val_list = [spacial_val]
availability_val_list = [availability_val]
entity_val_list = [entity_val]
correct_answer = "a"
batch_size = 5

training_data = [[spacial_val_list, availability_val_list, entity_val_list, correct_answer]] * 20


def get_list(training_data, batch_size):
    retrieved_entries = []
    for i in range(0, batch_size):
        index = randint(0, len(training_data)-1)
        #print(index)
        retrieved_entries.append(training_data[index])
    return retrieved_entries


def randomly_update_alpha(alpha_vec):
    index = randint(0, 2)
    for i in range(2):
        if index == i:
            alpha_vec[i] += learning_rate
        else:
            alpha_vec[i] -= learning_rate / 2
    return alpha_vec, index


def train(alpha_vec, training_data):
    for i in range(num_epochs):
        #print("epoch: " + str(i))

        # randomize which alpha value to change
        alpha_vec, index = randomly_update_alpha(alpha_vec)

        # test new alpha vec on 5 samples from training data
        validation_set = get_list(training_data, batch_size)

        validation_success_num = validation(validation_set, alpha_vec)
        #print(validation_success_num)

        if validation_success_num >= batch_size / 2:
            print("accuracy: ", validation_success_num / batch_size)
            alpha_vec = update_alpha(alpha_vec, index, 0)
        else:
            alpha_vec = update_alpha(alpha_vec, index, 1)
        #print("accuracy: ", validation_success_num/batch_size)
        #print("alpha vector: ", alpha_vec)



def get_compound_ranking(list_a, list_b, list_c, alpha_vec):
    curr_compound_ranking = {}
    #print(list_a)
    for key in list_a[0]:
        list_a_value = list_a[0][key]
        list_b_value = list_b[0][key]
        list_c_value = list_c[0][key]
        curr_compound_ranking[key] = list_a_value * alpha_vec[0] + list_b_value * alpha_vec[1] + list_c_value * \
                                     alpha_vec[2]

    return curr_compound_ranking


def validation(validation_set, alpha_vec):
    validation_success_num = 0

    for validation_data in validation_set:

        spacial_val_list = validation_data[0]
        availability_val_list = validation_data[1]
        entity_val_list = validation_data[2]
        correct_answer = validation_data[3]

        curr_compound_ranking = get_compound_ranking(spacial_val_list, availability_val_list, entity_val_list, alpha_vec)

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
    learning_rate = 0.005
    #print("validation index" ,index)
    if validation_result == 0:  # success
        print("validation success")
        print(alpha_vec)
        for i in range(3):
            if i == index:
                alpha_vec[i] += learning_rate
            else:
                alpha_vec[i] -= learning_rate / 2

    else:  # failed
        #print("validation failed")
        for i in range(3):
            if i == index:
                alpha_vec[i] -= learning_rate
            else:
                alpha_vec[i] += learning_rate / 2
    #print(alpha_vec)
    return alpha_vec



#!/usr/bin/env python
# coding: utf-8

# In[101]:


import random
import copy


# In[73]:


object_list = ["CounterDrawer","RubbishBin","Toaster","Microwave","Stove","Light","DishWasher","Refrigerator","Rice_Cooker","Oven","CoffeeMachine","ExtractorFan"]
instance_number=1
intent_dict={
    "Microwave":["open", "close", "start", "stop"],
    
    "Light":["turn_on", "turn_off"], 
    
    "Oven":["open", "close", "turn_on", "turn_off"], 
    
    "Rice_Cooker":["open", "close", "start", "stop"], 
    
    "Refrigerator":["open", "close", "turn_on", "turn_off"], 
    
    "Stove":["turn_on", "turn_off"],
    
    "CounterDrawer":["turn_on", "turn_off"],
    
    "Toaster":["turn_on", "turn_off"],
    
    "DishWasher":["open", "close", "turn_on", "turn_off"],
   
    "RubbishBin":["open", "close"],

    "CoffeeMachine":["open", "close", "turn_on", "turn_off"],

    "ExtractorFan":["turn_on", "turn_off"]

}

intent_search_dict={
    "Microwave":{"open":0, "close":0, "start":0, "stop":0},
    
    "Light":{"turn_on":0, "turn_off":0}, 
    
    "Oven":{"open":0, "close":0, "turn_on":0, "turn_off":0}, 
    
    "Rice_Cooker":{"open":0, "close":0, "start":0, "stop":0}, 
    
    "Refrigerator":{"open":0, "close":0, "turn_on":0, "turn_off":0}, 
    
    "Stove":{"turn_on":0, "turn_off":0},
    
    "CounterDrawer":{"turn_on":0, "turn_off":0},
    
    "Toaster":{"turn_on":0, "turn_off":0},
    
    "DishWasher":{"open":0, "close":0, "turn_on":0, "turn_off":0},
   
    "RubbishBin":{"open":0, "close":0},

    "CoffeeMachine":{"open":0, "close":0, "turn_on":0, "turn_off":0},

    "ExtractorFan":{"turn_on":0, "turn_off":0}

}
intent_list=[]
entity_list=[]
result_list=[]
# print(intent_dict)


# In[74]:


input1=[]
input2=[]
input3=[]
for item in object_list:
    for i in range(instance_number):
        input1.append(item+str(i+1))
        input2.append(item+str(i+1))
        input3.append(item+str(i+1))
        input1.append(item+str(i+1))
        input2.append(item+str(i+1))
        input3.append(item+str(i+1))
        input1.append(item+str(i+1))
        input2.append(item+str(i+1))
        input3.append(item+str(i+1))
        input1.append(item+str(i+1))
        input2.append(item+str(i+1))
        input3.append(item+str(i+1))
        input1.append(item+str(i+1))
        input2.append(item+str(i+1))
        input3.append(item+str(i+1))
        input1.append(item+str(i+1))
        input2.append(item+str(i+1))
        input3.append(item+str(i+1))
        input1.append(item+str(i+1))
        input2.append(item+str(i+1))
        input3.append(item+str(i+1))
        input1.append(item+str(i+1))
        input2.append(item+str(i+1))
        input3.append(item+str(i+1))
        input1.append(item+str(i+1))
        input2.append(item+str(i+1))
        input3.append(item+str(i+1))
        input1.append(item+str(i+1))
        input2.append(item+str(i+1))
        input3.append(item+str(i+1))
        input1.append(item+str(i+1))
        input2.append(item+str(i+1))
        input3.append(item+str(i+1))
        input1.append(item+str(i+1))
        input2.append(item+str(i+1))
        input3.append(item+str(i+1))
random.shuffle(input1)
random.shuffle(input2)
random.shuffle(input3)
# print(input1)
# print(input2)
# print(input3)


# In[75]:


gaze_list_temp=[]
for item in zip(input1, input2, input3):
    if len(item) == len(set(item)):
        gaze_list_temp.append(item)
print(gaze_list_temp)
print(len(gaze_list_temp))

# In[76]:


top_gaze_1=[]
top_gaze_2=[]
top_gaze_3=[]
for item in gaze_list_temp:
    top_gaze_1.append(item[0])
    top_gaze_2.append(item[1])
    top_gaze_3.append(item[2])
   
print(top_gaze_1)
print(top_gaze_2)
print(top_gaze_3)



# In[78]:


temp=[list(item) for item in gaze_list_temp]
# print(temp)
for item in temp:
    intent_temp=random.choice(intent_dict[random.choice(item)[:-1]])
    intent_list.append(intent_temp)
    item.append("None")
    entity_temp=random.choice(item)
    

    if entity_temp!="None":
        entity_list.append(entity_temp[:-1])
        result_temp=[]
        for item_temp in item:
            if item_temp==entity_temp:
                result_temp.append(item_temp)
        result_list.append(random.choice( result_temp))
    else:
        entity_list.append(entity_temp)
        result_temp=[]
        for item_temp in item:
            if item_temp[:-1] in intent_dict.keys():
                for item_temp_dict in intent_dict[item_temp[:-1]]:
                    if item_temp_dict == intent_temp:
                        result_temp.append(item_temp)
    
        result_list.append(random.choice(result_temp))
    
#print(temp)
print(intent_list)
print(entity_list)
print(result_list)


# In[102]:


gaze_object_list=[]

for i in range( len(object_list)):
    gaze_object_list.append([])
    
print(gaze_object_list)

for item in gaze_list_temp:
    
    for ref_index in range(len(object_list)):
        if item[0][:-1] == object_list[ref_index]:
            gaze_object_list[ref_index].append("yes")
        elif item[1][:-1] == object_list[ref_index]:
            gaze_object_list[ref_index].append("yes")
        elif item[2][:-1] == object_list[ref_index]:
            gaze_object_list[ref_index].append("yes")  
        else:
            gaze_object_list[ref_index].append("no") 

print(gaze_object_list)
print(len(gaze_object_list[0]))



# In[103]:


intent_action_list=[]

for i in range( len(object_list)):
    intent_action_list.append([])
    
print(intent_action_list)

for item in intent_list:
    
    for inf_index in range(len(object_list)):
        if item in intent_search_dict[object_list[inf_index]].keys():
            intent_action_list[inf_index].append("yes")
        
        else:
            intent_action_list[inf_index].append("no")

print(intent_action_list)
print(len(intent_action_list[0]))


# In[104]:


entity_object_list=[]

for i in range( len(object_list)):
    entity_object_list.append([])
    
print(entity_object_list)

for item in entity_list:
    
    for enf_index in range(len(object_list)):
        if item == object_list[enf_index]:
            entity_object_list[enf_index].append("yes")
        
        else:
            entity_object_list[enf_index].append("no")

print(entity_object_list)
print(len(entity_object_list[0]))


# In[105]:


# initial


# In[ ]:





# In[ ]:





# In[106]:


# print(len(result_list))
# print(0.7*len(result_list))
# whole=len(result_list)
# tran_len=int(0.7*whole)
# infer_len=whole-tran_len
# print(whole)
# print(tran_len)
# print(infer_len)


# In[107]:


from sklearn import preprocessing
from sklearn.naive_bayes import GaussianNB

# t_top_gaze_1= top_gaze_1[0:tran_len]
# t_top_gaze_2= top_gaze_2[0:tran_len]
# t_top_gaze_3= top_gaze_3[0:tran_len]
# in_top_gaze_1=top_gaze_1[tran_len:whole]
# in_top_gaze_2=top_gaze_2[tran_len:whole]
# in_top_gaze_3=top_gaze_3[tran_len:whole]
# t_intent_list=intent_list[0:tran_len]
# in_intent_list=entity_list[tran_len:whole]


# t_entity_list=entity_list[0:tran_len]
# in_entent_list=entity_list[tran_len:whole]

# t_result_list=result_list[0:tran_len]
# in_result_list=result_list[tran_len:whole]

#creating labelEncoder
le = preprocessing.LabelEncoder()
# Converting string labels into numbers.
# top_first_gaze_object_encoded=le.fit_transform(top_gaze_1)

# top_second_gaze_object_encoded=le.fit_transform(top_gaze_2)

# top_third_gaze_object_encoded=le.fit_transform(top_gaze_3)
# print(top_first_gaze_object_encoded)

# # top_second_gaze_object_encoded = [item+20 for item in top_second_gaze_object_encoded]
# # print(top_second_gaze_object_encoded)
# # top_third_gaze_object_encoded = [item+30 for item in top_third_gaze_object_encoded]
# # print(top_third_gaze_object_encoded)
# # In[67]:


# # Converting string labels into numbers
# extracted_intent_encoded=le.fit_transform(intent_list)
# extracted_entity_encoded=le.fit_transform(entity_list)
encode_gaze=[]
for item in gaze_object_list:
  
    encode_gaze.append( le.fit_transform(item))
    
print(encode_gaze)

encode_intent=[]
for item in intent_action_list:
    encode_intent.append( le.fit_transform(item))
    
print(encode_intent)

encode_entity=[]
for item in entity_object_list:
    encode_entity.append( le.fit_transform(item))
    
print(encode_entity)

label=le.fit_transform(result_list)

print(label)

temp_feature=[]

temp_feature.extend(encode_gaze)

temp_feature.extend(encode_intent)
temp_feature.extend(encode_entity)

print(temp_feature)

features=[list(item) for item in zip(*temp_feature)]

print(list(features))


# In[108]:


# print(tran_len)
# t_features=features[0:tran_len]
# v_features=features[tran_len:whole]

# t_labels=label[0:tran_len]
# v_labels=label[tran_len:whole]
# print(t_labels)
# label_dict={}

# for i in range(whole):
#     label_dict[label[i]]=result_list[i]
# print(label_dict)


# In[109]:





# # In[69]:


# #Create a Gaussian Classifier
# model = GaussianNB()
# # x = np.array(list(features))
# # y= np.array(label)
# # Train the model using the training sets
# model.fit(t_features,t_labels)




# # In[ ]:


# In[110]:


# #Predict Output
# temp_final=[]
# for item in features:
#     predicted= model.predict([item]) # 0:Overcast, 2:Mild
#     temp_final.append(predicted[0])
# print(temp_final)


# In[111]:


from sklearn.model_selection import train_test_split

# Split dataset into training set and test set
X_train, X_test, y_train, y_test = train_test_split(features, label, test_size=0.3,random_state=109) # 70% training and 30% test
print(X_train)


# In[112]:


from sklearn.naive_bayes import GaussianNB


gb_model = GaussianNB()


gb_model.fit(X_train, y_train)


y_pred = gb_model.predict(X_test)


# In[113]:


from sklearn import metrics


print("Accuracy:",metrics.accuracy_score(y_test, y_pred))


# In[ ]:





# In[ ]:





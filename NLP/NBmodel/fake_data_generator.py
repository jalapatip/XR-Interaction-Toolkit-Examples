#!/usr/bin/env python
# coding: utf-8

# In[72]:


import random
import copy


# In[73]:


object_list = ["CounterDrawer","RubbishBin","Toaster","Microwave","Stove","Light","DishWasher","Refrigerator","Rice_Cooker","Oven","CoffeeMachine","ExtractorFan"]
instance_number=3
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
intent_list=[]
entity_list=[]
result_list=[]
print(intent_dict)


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
random.shuffle(input1)
random.shuffle(input2)
random.shuffle(input3)
print(input1)
print(input2)
print(input3)


# In[75]:


gaze_list_temp=[]
for item in zip(input1, input2, input3):
    if len(item) == len(set(item)):
        gaze_list_temp.append(item)
print(gaze_list_temp)


# In[76]:


top_gaze_1=[]
top_gaze_2=[]
top_gaze_3=[]
for item in gaze_list_temp:
    top_gaze_1.append(item[0])
    top_gaze_2.append(item[0])
    top_gaze_3.append(item[0])
print(top_gaze_1)


# In[78]:


temp=[list(item) for item in gaze_list_temp]

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
    
print(temp)
print(intent_list)
print(entity_list)
print(result_list)


# In[ ]:





# In[ ]:





# In[ ]:





# In[ ]:





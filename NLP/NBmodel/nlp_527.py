#!/usr/bin/env python
# coding: utf-8

# In[64]:


from sklearn import preprocessing
from sklearn.naive_bayes import GaussianNB


# In[65]:



top_first_gaze_object=['Microwave1','Oven1',"Oven1 "]
top_second_gaze_object=['Microwave2','Oven2', "Oven2"]
top_third_gaze_object=['Oven1','Microwave1', "Microwave2"]
extracted_intent=['Open','Open',"turn on"]

extracted_entity =['Microwave','Oven1', "none"]

result_action=["Microwave1", "Oven1", "Oven1" ]


# In[66]:



#creating labelEncoder
le = preprocessing.LabelEncoder()
# Converting string labels into numbers.
top_first_gaze_object_encoded=le.fit_transform(top_first_gaze_object)
top_second_gaze_object_encoded=le.fit_transform(top_second_gaze_object)
top_third_gaze_object_encoded=le.fit_transform(top_third_gaze_object)


# In[67]:


# Converting string labels into numbers
extracted_intent_encoded=le.fit_transform(extracted_intent)
extracted_entity_encoded=le.fit_transform(extracted_entity)


label=le.fit_transform(result_action)
print (extracted_intent_encoded)
print (extracted_entity_encoded)
print(label)


# In[68]:


features=[list(item) for item in zip(top_first_gaze_object_encoded,top_second_gaze_object_encoded, top_third_gaze_object_encoded ,extracted_intent_encoded, extracted_entity_encoded)]

print(list(features))


# In[69]:


#Create a Gaussian Classifier
model = GaussianNB()
# x = np.array(list(features))
# y= np.array(label)
# Train the model using the training sets
model.fit(features,label)

#Predict Output
predicted= model.predict([[0,0,2, 0, 0]]) # 0:Overcast, 2:Mild
print( predicted)


# In[ ]:





# In[ ]:





# In[ ]:





# In[ ]:





# In[ ]:





# In[ ]:





import pandas as pd
import numpy as np
import re

def clean_text_with_regex(text_array):
    cleaned = []
    for text in text_array:
        
        text = text.strip().lower()
        
        text = re.sub(r'[^a-zа-яё0-9\s]', '', text)
        
        text = re.sub(r'\s+', ' ', text).strip()
        cleaned.append(text)
    
    return cleaned

dictionary = pd.read_excel("Dictionary.xlsx")
text_data = pd.read_excel("Data.xlsx")

result = clean_text_with_regex(text_data["Описание"])
X = pd.DataFrame({"Описание": result})


n = len(X["Описание"])
m = len(dictionary["Токен"])

mas_X = np.zeros((n, m))


for i in range(len(X["Описание"])):
    for j in range(len(dictionary["Токен"])):
        if dictionary["Токен"][j] in X['Описание'][i]:
            mas_X[i][j] = 1
            print(i)

Y = pd.DataFrame(text_data["Рабочая группа"].values)

X = pd.DataFrame(mas_X)
X.to_json("X.json")
Y.to_json("Y.json")



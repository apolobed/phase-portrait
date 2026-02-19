import numpy as np
import tensorflow as tf
from sklearn.model_selection import train_test_split
from sklearn.preprocessing import StandardScaler, LabelEncoder
import pandas as pd
import re

def clean_text(text):
    cleaned = ''

    text = text.strip().lower()

    text = re.sub(r'[^a-zа-яё0-9\s]', '', text)

    text = re.sub(r'\s+', ' ', text).strip()
    cleaned = text

    return cleaned

dictionary = pd.read_excel("Dictionary.xlsx")
working_groups = pd.read_excel("Рабочие группы.xlsx")
text_data = pd.read_excel("Тестовые наряды.xlsx")

index_of_text_data = 1001
example = text_data["Описание"][index_of_text_data]

cleaned_text = clean_text(example)

m = len(dictionary["Токен"])

mas_words_in_text = [0]*m

for i in range(len(dictionary["Токен"])):
    if dictionary["Токен"][i] in cleaned_text:
        mas_words_in_text[i] = 1

processed_text = np.array(mas_words_in_text)

model = tf.keras.models.load_model("classification_model.h5")

def predict(model, input_features):
    input_array = np.array([input_features])

    prediction = model.predict(input_array, verbose=0)

    predicted_class = np.argmax(prediction[0])
    confidence = np.max(prediction[0]) * 100

    return predicted_class, confidence, prediction[0]


sample_features = processed_text
predicted_class, confidence, probs = predict(model, sample_features)
predicted_class = predicted_class + 1

predicted_group = ""
for i in range(len(working_groups["Рабочая группа"])):
  if predicted_class == working_groups["Индекс"][i]:
    predicted_group = working_groups["Рабочая группа"][i]

print(cleaned_text)
print(f"Предсказанный класс: {predicted_class}")
print(f"Рабочая группа: {predicted_group}")
print(f"Уверенность: {confidence:.2f}%")
print(f"Реальная рабочая группа: {text_data["Рабочая группа"][index_of_text_data]}")
print("\n")
print(f"Все вероятности: {probs}")
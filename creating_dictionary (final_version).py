from sklearn.feature_extraction.text import CountVectorizer
import numpy as np
import pandas as pd

def sklearn_text_preparation(texts, max_features=3000, max_length=50):
    vectorizer = CountVectorizer(
        max_features=max_features,
        lowercase=True,
        token_pattern=r'\b\w+\b'
    )
    
    # Преобразуем в матрицу встречаемости слов
    X = vectorizer.fit_transform(texts)
    
    # Можно преобразовать в плотную матрицу
    X_dense = X.toarray()
    
    return X_dense, vectorizer

text_data = pd.read_excel("Data.xlsx")

text_mas = text_data['Описание'].to_numpy()

X, vectorizer = sklearn_text_preparation(text_mas)

dictionary = pd.DataFrame({"Токен": vectorizer.get_feature_names_out()})
dictionary.to_excel("Old_dictionary.xlsx", index=False)


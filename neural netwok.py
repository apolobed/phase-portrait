import numpy as np
import tensorflow as tf
from sklearn.model_selection import train_test_split
from sklearn.preprocessing import StandardScaler, LabelEncoder
import matplotlib.pyplot as plt
import pandas as pd

df1 = pd.read_json("X.json")
df2 = pd.read_json("Y.json")

X = df1.to_numpy()
Y_test = df2.to_numpy()
Y = Y_test.flatten()



# Сюда нужно будет подставить 
#####################################################

# Генерация случайных данных для примера
# В реальном случае вы бы загружали свои данные


# 1. ПОДГОТОВКА ДАННЫХ

# Создаем случайные данные: 100 признаков, 1000 объектов
n_features = len(X[0])
n_samples = len(Y)
n_classes = 17  # Пусть у нас будет 5 классов

# Генерируем признаки (X)


# Генерируем метки классов (Y) - 5 различных классов


#######################################################



# Кодируем метки в one-hot формат
label_encoder = LabelEncoder()
Y_encoded = label_encoder.fit_transform(Y)
Y_onehot = tf.keras.utils.to_categorical(Y_encoded, n_classes)

# Разделяем данные на обучающую и тестовую выборки
X_train, X_test, y_train, y_test = train_test_split(
    X, Y_onehot, test_size=0.2, random_state=42, stratify=Y
)


# 2. СОЗДАНИЕ МОДЕЛИ НЕЙРОННОЙ СЕТИ

model = tf.keras.Sequential([
    # Входной слой: 100 признаков на входе
    tf.keras.layers.Input(shape=(n_features,)),
    
    # Первый скрытый слой с 64 нейронами и функцией активации ReLU
    tf.keras.layers.Dense(64, activation='relu', name='hidden_layer_1'),
    
    # Добавляем Dropout для регуляризации (предотвращение переобучения)
    tf.keras.layers.Dropout(0.3),
    
    # Второй скрытый слой с 32 нейронами
    tf.keras.layers.Dense(32, activation='relu', name='hidden_layer_2'),
    
    # Еще один Dropout
    tf.keras.layers.Dropout(0.3),
    
    # Выходной слой: 5 нейронов (по количеству классов) с softmax
    tf.keras.layers.Dense(n_classes, activation='softmax', name='output_layer')
])

# 3. КОМПИЛЯЦИЯ МОДЕЛИ
print("\n3. Компиляция модели...")

model.compile(
    optimizer='adam',           # Адаптивный оптимизатор
    loss='categorical_crossentropy',  # Функция потерь для многоклассовой классификации
    metrics=['accuracy']        # Метрика для оценки - точность
)

# 4. ОБУЧЕНИЕ МОДЕЛИ
print("\n4. Обучение модели...")

history = model.fit(
    X_train, y_train,
    epochs=50,                   # Количество эпох обучения
    batch_size=32,               # Размер мини-батча
    validation_split=0.2,        # Часть данных для валидации
    verbose=1,                   # Вывод прогресса обучения
    callbacks=[
        # Ранняя остановка, если качество на валидации не улучшается
        tf.keras.callbacks.EarlyStopping(
            monitor='val_loss',
            patience=10,
            restore_best_weights=True
        )
    ]
)

# 5. ОЦЕНКА МОДЕЛИ
print("\n5. Оценка модели...")

# Оценка на тестовых данных
test_loss, test_accuracy = model.evaluate(X_test, y_test, verbose=0)
print(f"Точность на тестовых данных: {test_accuracy:.4f}")
print(f"Потери на тестовых данных: {test_loss:.4f}")

# 7. ПРИМЕР ПРЕДСКАЗАНИЙ
print("\n7. Пример предсказаний...")

# Делаем предсказания на тестовых данных
predictions = model.predict(X_test[:5])  # Первые 5 примеров

print("\nПримеры предсказаний:")
for i, pred in enumerate(predictions):
    predicted_class = np.argmax(pred)
    true_class = np.argmax(y_test[i])
    confidence = np.max(pred) * 100
    
    print(f"Объект {i+1}:")
    print(f"  Предсказанный класс: {predicted_class}")
    print(f"  Истинный класс: {true_class}")
    print(f"  Уверенность: {confidence:.2f}%")
    print(f"  Верно: {'Да' if predicted_class == true_class else 'Нет'}")

# 8. СОХРАНЕНИЕ И ЗАГРУЗКА МОДЕЛИ (ОПЦИОНАЛЬНО)
print("\n8. Сохранение модели...")

# Сохраняем модель
model.save('classification_model.h5')
print("Модель сохранена как 'classification_model.h5'")

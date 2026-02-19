import pandas as pd

text_data = pd.read_excel("Выгрузка нарядов (01.07.2025-31.12.2025).xlsx")
working_groups = pd.read_excel("рабочие группы.xlsx")

for i in range(len(text_data["Рабочая группа"])):
    if "ГТП" in text_data["Рабочая группа"][i]:
        text_data.loc[i, "Рабочая группа"] = "ГТП"

text_data_data = text_data[text_data["Описание"].apply(lambda x: type(x) == str)]

group_dictionary = dict(zip(working_groups["Рабочая группа"], working_groups["Индекс"]))

text_data["Рабочая группа"] = text_data["Рабочая группа"].map(group_dictionary)

text_data = text_data[text_data["Рабочая группа"].apply(lambda x: pd.notna(x))]

text_data.to_excel("Data.xlsx", index=False)


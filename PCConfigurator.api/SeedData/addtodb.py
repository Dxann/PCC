import json

# Читаем данные из JSON файла
with open('cpu.json', 'r', encoding='utf-8') as file:
    data = json.load(file)

sql_values = []
for item in data:
    values = [
        f"'{item['Name']}'",
        f"'{item['Manufacturer']}'",
        str(item['Price']),
        f"'{item['ImageUrl']}'",
        f"'{item['ShortDescription'].replace("'", "''")}'",
        f"'{item['Socket']}'",
        str(item['Cores']),
        str(item['Threads']),
        str(item['BaseFrequency']),
        str(item['BoostFrequency']),
        str(item['TDP']),
        f"'{item['IntegratedGraphics']}'"
    ]
    sql_values.append(f"({', '.join(values)})")

sql = "INSERT INTO CPUs (Name, Manufacturer, Price, ImageUrl, ShortDescription, Socket, Cores, Threads, BaseFrequency, BoostFrequency, TDP, IntegratedGraphics) VALUES\n" + ",\n".join(sql_values) + ";"

print(sql)
import json

with open('thermalpaste.json', 'r', encoding='utf-8') as file:
    data = json.load(file)

for item in data:
    name_with_underscores = item['Name'].replace(' ', '_')
    item['ImageUrl'] = f"/images/thermalpaste/{name_with_underscores}"

with open('thermalpaste.json', 'w', encoding='utf-8') as file:
    json.dump(data, file, ensure_ascii=False, indent=4)

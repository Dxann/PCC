import os
import re

directory = os.path.dirname(os.path.realpath(__file__))
script_name = 'python.py'
filenames = os.listdir(directory)

def remove_all_numeric_suffixes(name):
    base, ext = os.path.splitext(name)
    pattern = re.compile(r'(_\d+)+$')
    while True:
        match = pattern.search(base)
        if not match:
            break
        base = base[:match.start()]
    return base + ext

for filename in filenames:
    if filename == script_name:
        continue

    new_name = filename.replace(' ', '_')
    cleaned_name = remove_all_numeric_suffixes(new_name)

    base, ext = os.path.splitext(cleaned_name)
    unique_name = cleaned_name
    count = 1

    # Обеспечиваем уникальность имени
    while (unique_name in filenames or os.path.exists(os.path.join(directory, unique_name))) and unique_name != filename:
        unique_name = f"{base}_{count}{ext}"
        count += 1

    if unique_name != filename:
        os.rename(os.path.join(directory, filename), os.path.join(directory, unique_name))

print("Переименование выполнено: удалены все числовые суффиксы.")

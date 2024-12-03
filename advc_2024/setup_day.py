import argparse
import glob
import os
import shutil
import requests

SESSION_COOKIE="53616c7465645f5f7509ccafdaef8e2c4e4a2f435d525dfeb75069ee7e97b901a916d754ade0f5524f12b0abb7fc6fe3a0ec09b4c3f157c8e88edcc1cae270ad"
YEAR="2024"
EXIT_ON_EXIST=True

def exit_if_needed(error_code):
    if EXIT_ON_EXIST:
        print(f"[ERROR!] Program exits with code {error_code}")
        exit(error_code)

def copy_tree(src, dst):
    if not os.path.exists(dst):
        shutil.copytree(src, dst)
        print(f"* Copied {src} to {dst}")
    else:
        print(f"! Destination folder {dst} already exists")
        exit_if_needed(1)

def download_input_data(dst_data_folder, day):
        url = f"https://adventofcode.com/{YEAR}/day/{int(day)}/input"
        headers = {"Cookie": f"session={SESSION_COOKIE}"}
        response = requests.get(url, headers=headers)
        
        filename = os.path.join(dst_data_folder, "input.txt")
        if response.status_code == 200:
            with open(filename, "w") as file:
                file.write(response.text)
            print(f"* Downloaded input data and saved to {filename}")
        else:
            print(f"[ERROR!] Failed to fetch input data: {response.status_code}")    

def change_day_value(dst_java_folder, day) :
    for java_file_path in glob.glob(os.path.join(dst_java_folder, "*.java")):
        with open(java_file_path, "r") as file:
            content = file.read()

        # Replace '00' with the given day
        modified_content = content.replace("00", day).replace("_template", "")

        with open(java_file_path, "w") as file:
            file.write(modified_content)

        print(f"* Updated 00 to {day} in {java_file_path}")


def add_app_calling(app_java_file, day) :
    with open(app_java_file, "r") as file:
        lines = file.readlines()

    # Find the correct insertion point (end of similar lines block)
    insertion_index = None

    for i, line in enumerate(lines):
        if f"new advc_{YEAR}.day" in line:
            if f"new advc_{YEAR}.day{day}" in line:
                print(f"! Line for day{day} already exists")
                exit_if_needed(2)
                return

            insertion_index = i + 1

    if insertion_index is not None:
        # Insert the new line
        new_line = f"        new advc_{YEAR}.day{day}.Solution().run();\n"
        lines.insert(insertion_index, new_line)

        # Write changes back to App.java
        with open(app_java_file, "w") as file:
            file.writelines(lines)

        print(f"* Added {new_line.strip()} to {app_java_file}")
    else:
        print("[ERROR!] Could not find insertion point in App.java file.")


if __name__ == "__main__":
    # Argument parsing
    parser = argparse.ArgumentParser(description="Set up folders and modify files for a specific day.")
    parser.add_argument("day", type=str, help="2-digit day string (e.g., 09)")
    parser.add_argument("--ignore_existing", action="store_true", help="If true, program does not exit on existing folder")

    args = parser.parse_args()
    day = args.day
    EXIT_ON_EXIST = not args.ignore_existing

    src_data_template = "app/data/day00_template"
    dst_data_folder = f"app/data/day{day}"
    src_java_template = f"app/src/main/java/advc_{YEAR}/day00_template"
    dst_java_folder = f"app/src/main/java/advc_{YEAR}/day{day}"
    app_java_file = f"app/src/main/java/advc_{YEAR}/App.java"
    
    copy_tree(src_data_template, dst_data_folder)
    copy_tree(src_java_template, dst_java_folder)

    download_input_data(dst_data_folder, day)
    change_day_value(dst_java_folder, day)
    add_app_calling(app_java_file, day)    

import os
import subprocess
import shutil

# Get the documentation folders
path_to_docs = "../documentation/"
files = [f for f in os.listdir(path_to_docs + "src/") if not os.path.isfile(os.path.join(path_to_docs + "src/", f)) and f != "template"]

# For each folder, move the main.pdf to documentation directory
for file in files:
    source = path_to_docs + "src/" + file
    
    # Check if main.pdf exists
    file_exists = os.path.exists(source + "/main.pdf")
    
    if not file_exists:
        # Check to see if main.tex exists (in this case, compile the latex)
        file_exists = os.path.exists(source + "/main.tex")
        
        if file_exists:
            subprocess.call(f"cd {source}/ && pdflatex main.tex", shell=True, stdout=subprocess.DEVNULL, stderr=subprocess.STDOUT)
            shutil.copyfile(source + "/main.pdf", path_to_docs + file + ".pdf")
            print(f"Compiled latex file and copied contents into {file}.pdf")
        else:
            print(f"main.tex not found in {file} folder.")
    
    else:
        # Move main.pdf to documentation
        # Change the filename to match the directory
        shutil.copyfile(source + "/main.pdf", path_to_docs + file + ".pdf")
        print(f"Successfully copied {file}.pdf.")

# if __name__ == "__main__":
#     username, server_name, server_port = sys.argv[1], sys.argv[2], int(sys.argv[3])
#     start_client(username, server_name, server_port)
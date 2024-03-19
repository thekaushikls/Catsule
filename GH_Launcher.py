# - - - - IMPROTS
import os, subprocess, json
from tkinter import Tk, filedialog

# - - - - CLASS LIBRARY
class GH_Launcher(object):

    CONFIG_FILE = "GH_Launcher.config"

    @staticmethod
    def save_rhino_path(rhino_path):
        #config = {"rhino_path": rhino_path}
        config = {"rhino_path": rhino_path.replace('/', '\\')}
        with open(GH_Launcher.CONFIG_FILE, "w") as f:
            json.dump(config, f)

    @staticmethod
    def get_rhino_path():
        if os.path.exists(GH_Launcher.CONFIG_FILE):
            with open(GH_Launcher.CONFIG_FILE, "r") as f:
                config = json.load(f)
                rhino_path = config.get("rhino_path")
                if rhino_path and os.path.exists(rhino_path):
                    return rhino_path
        else:
            root = Tk()
            root.withdraw()
            rhino_path = filedialog.askopenfilename(
                title="Browse Rhino.exe",
                filetypes=[("Rhino Application", "Rhino.exe")])
            if rhino_path:
                GH_Launcher.save_rhino_path(rhino_path)
        
            return rhino_path
    
    @staticmethod
    def launch():
        rhino_path = GH_Launcher.get_rhino_path()
        if rhino_path:
            cmd = r'""{}" /nosplash /runscript="_-Grasshopper Banner Disable Window Load Window Show _Enter""'.format(rhino_path)
            os.system(cmd)
        else:
            print("Rhino.exe not found.")
            exit(1)

# - - - - RUNSCRIPT
if __name__ == "__main__":
    GH_Launcher.launch()
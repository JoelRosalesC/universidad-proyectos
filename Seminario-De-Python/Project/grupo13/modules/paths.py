from pathlib import Path

ROOT_DIR = Path(__file__).resolve().parent.parent

DATASETS_DIR = ROOT_DIR / 'datasets'

MODIFIED_DATASETS = ROOT_DIR / 'datasets_procesados'

AR_AIRPORTS_DATA = DATASETS_DIR / 'ar-airports.csv'

AR_AIRPORTS_DATA_MODIFIED = MODIFIED_DATASETS / 'ar-airports_modified.csv'

AR_DATA = DATASETS_DIR / 'ar.csv'

C2022_DATA = DATASETS_DIR / 'c2022_tp_c_resumen_adaptado.csv'

C2022_DATA_MODIFIED = MODIFIED_DATASETS / 'c2022_modified.csv'

CONNECTIONS_DATA = DATASETS_DIR / 'Conectividad_Internet.csv'

CONNECTIONS_DATA_MODIFIED = MODIFIED_DATASETS / 'Conectividad_Internet_m.csv'

AR_LAKES = DATASETS_DIR / 'lagos_arg.csv'

AR_LAKES_MODIFIED = MODIFIED_DATASETS / 'lagos_arg.csv'

USER_DATA_JSON = ROOT_DIR / 'app' / 'user_register' / 'users.json'

PLAYED_GAMES_JSON = ROOT_DIR / 'app' / 'played_games.json' 

if __name__ == "__main__":
    print(f"Direccion base: {ROOT_DIR} ")
    print(f"Direccion datasets y datasets modificados: \n {DATASETS_DIR} \n {MODIFIED_DATASETS}")
    print(f"Direccion ejemplo: {C2022_DATA}")

# Az étterem projekt leírása (specifikáció)

## Képernyőképek
Képernyőképeket (egyenlőre) nem készítünk, mert az MVC template-k elkészítik nekünk a képernyőket, azt nem tudjuk befolyásolni. Így amit kapunk, azt fogjuk használni.

## Szereplők
### Érdeklődő
### Étlap
#### Példa étlap
- Pizzák
--------
    - Margarita pizza 200 Ft
        mozzarella sajt, pizza szósz
    - Hawaii pizza 300 Ft
        sonka, ananász. mozzarella sajt, pizza szósz
- Italok
--------
    - Ásványvíz 100 Ft (3 dl)
    - Cola 120 Ft (3 dl)

## Forgatókönyvek
### Érdeklődő eldönti, hogy akar-e nálunk enni?
Érdeklődő elkéri az étlapot és megnézi, hogy mit lehet nálunk enni, és mennyiért.


# Code First Migration
kell hozzá:
- EntityFramework nuget package
- a Code First Migration engedélyezése: a Package Manager Console-ból: 
  - enable-migrations
  - ha létezik adatbázis, akkor automatikusan létrejön az első MigrationStep, ha nem létezik,
    akkor kell az add-migration
  - add-migration 'Initial Migration'
  - update-database

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
- az adatbázist az SQL server object explorer ablakban látjuk, localdb-ként

## Saját adat adatbázisba tétele lépései
- Létre kell hozni egy osztályt, ami az adatokat tartalmazza (pl. Category)
- Az osztályt fel kell venni DbSet típusú property-ként az ApplicationDbContext osztályba.
- Ki kell adni az Add-Migration parancsot.
- Ki kell adni az Update-Database parancsot. 

## Adatbázis helyének megváltoztatása
- A web.config-ban a ConnectionStrings beállítást kell átírni, a minták a https://wwwconnectionstrings.com
  oldalon

## Kezdeti automatikus adatfeltöltés
- Migrations / Configurations.cs-ben
- Minden update-database esetén lefut (Seed metódus)

## Lenyílómező kezelése
- Entry - kapu az Entity Framework-be
- A MenuItem mellé a Category-t is be kell tölteni, hogy módosítani tudjuk a menuItem kategóriáját

## Felhasználók azonosítása és jogosultságkezelés
- Bevezető rész: azonosítjuk a felhasználóinkat, és elválasztjuk a bejelentkezett és be nem jelentkezett felhasználókat egymástól
- Az ASP.NET Identity a következőket végzi:
  - lehetővé teszi a felhasználók regisztrációját az oldalon
  - a regisztrált felhasználóknak végzi a session kezelését (session: bejelentkezéstől kijelentkezésig terjedő tevékenység)
  - le tudjuk kérdezni egy kérés kiszolgálásakor, hogy a felhasználó be van-e jelentkezve, és ha igen, akkor mi az ő neve
  - a controllereket és az action-öket védeni tudjuk: megadhatjuk, hogy csak bejelentkezett felhasználók férjenek hozzá
    a teljes controllerhez vagy az egyes action-ökhőz
- Authentikáció: felhasználó azonosítás (bejelentkezés, session kezelés, kijelentkezés)
- Authorizáció: az adott felhasználónak van-e joga az adott tevékenységre (Controller/Action)
- Az ASP.NET Identity alapértelmezettten Roles Based Authorizációval foglalkozik. Ez azt jelenti, hogy a felhasználókat csoportokba
  tudjuk rendezni és a csoportoknak tudok jogosultságokat biztosítni.
  Például: fel tudok venni Admin, Pincér, Szakács, Főpincér csoportokat, és az egyes felhasználókat fel tudom venni ezekbe a csoportokba.
  Majd azt tudom mutatni, hogy egy adott Controller/Action egy adott csoport számára használható-e.

### Csoport (Role) létrehozás
- Az AspNetRole táblába felvisszük a csoportok neveit (az id mező GUID-ot vár, például a https://www.guidgenerator.com/online-guid-generator.aspx online Guid generátorral tudnk létrehozni)
- Regisztrálunk felhasználókat, ezek az AspNetUsers táblába kerülnek
- A felhasználókat hozzá tudjuk adni a csoporthoz úgy, hogy az AspNetUsers.Id és az AspNetRoles.Id értéket felvesszük az AspNetUserRoles táblába, megfelelően párosítva.
- Figyelem: a jogosultságokat cookie-ba bejelentkezéskor írja az ASP.NET, ezért, ha változik valami, akkor ki kell jelentkezni és újra be

## Saját megjelenítő és szerkesztő HTML template
A cél: kiemelni az azonos küdor egy külön állományba, majd különböző helyekről használni.
- A View\Shared\EditorTemplates mappában vannak a szerkesztésre használt template nézetek.
- Ha kiadjuk a @Html.EidtorForModel() utasítást a view-n, akkor az ASP.NET ezek közül megkeresi azt, ami a modelhez tartozik.
- Ha nincs ilyen, akkor generál egy alapértelmezettet.
- Ha van (MenuItem.cshtml a MenuItem osztálynak), akkor beemeli az aktuális helyre az utasítás helyére

## Saját modellek, modellek szerepe
- Adatcsomagok kezelésére használható.
- Minden ami adat az tulajdonképpen "modell", ez a szakzsargonban szinonim fogalom.
- Névterek - osztályok, objektumok csoportosítására használjuk.
- Az objektumokból rengeteg van, több tízezer, mi is akármennyit létre tudunk hozmi. Ezért egy listában ezeket nem lehet kezelhetően használni.
  Erre találták ki a névtereket (namespace).
- System  - Data   - Entity
          - Linq
          - Net
          - Web    - MVC
- A mi névterünk egy része:
  - OopRestaurant201810   - Controllers   - MenuItemsController
                                          - CategoriesController 
                          - Models
- namespace CsoportosításNév
  {
    class Osztálynév {
    }
  }
  var valami = CsoportosításNév.Osztálynév;

  vagy a kódblokk elején:

  using CsoportosításNév
  var valami = Osztálynév();

- A névtér feletti névteret is látom, így a felül lévőt nem kell megadni.

## Modell osztályok elnevezése
- Ha ki akarjuk hangsúlyozni, hogy valami modell, akkor mindenképpen szerepel a nevében a "models"
- A model (DataModel) az az osztály, ami az adattárolásban részt vesz. Vagy ő az az osztály, amibe az adattárolásból betöltjük az adatokat.
- Nincs kötelező szabály az elnevezéseknél.
- IdentityModels - egy állományba kerüljön az összes olyan adatmodellhez köthető dolog ami az Identity kezeléshez kapcsolódik.
- Az osztályokat külön állományokba kell tenni.
- AccountViewModels, ManageViewModels - csoportosító állományok.
- ViewModel az a model osztály, ami csak a megjelenítésben vesz részt, nem megy el az adatbázisig.
- A controller állítja elő és átadja a view felé.
- Mi un. hibrid modellt gyártunk, ami mindenben részt vesz.

- Ha szét akarnánk szedni a szerepeket, akkor lenne:
  - egy Category osztály, ami a Code First segítségével az adatbázist "jelentené"
  - egy CategoryModel osztály, amibe az adatbázisból olvasott adatokat beírnánk
  - egy CategoryViewModel, amit a Controller gyárt és átad a View-nak, illetve a Controller fogad a HTML form-ról és gyárt belőle CategoryModel-t,
    amit aztán Category-vá alakítunk és az adatbázisba kiírjuk

## Nézet generálása DisplayTemplates segítségével
- MenuItem layout
  - Create / Edit -> EditorForModels -> EditorTemplates könyvtárban a MenuItem
  - Detail / Delete -> DisplayForModels -> DisplayTemplates könyvtárban a MenuItem

## CRUD műveletek szervezése a Controlleren
- Create, Read, Update, Delete
- New - újonnan megjelenítendő elem
### Megjelenítő műveletek (Get action-ök)
- Details
- Create
- Edit
- Delete
Ezekben közös, hogy megjelenítő oldalakat generálnak
### Adat(bázis) műveletek
- Read
- New

### Módosító műveletek (Post action-ök)
- Create
- Edit
- Delete
Ezekben a műveletekben közös, hogy a beérkező adatokat elmentik:
- Create
- Update
- Delete

## Gyakorlás és ismétlás
- Asztalok és csoportosításuk: az asztalfoglalásnak és a rendelésfelvételnek/számlázásnak is alapja, hogy a vendégek asztaloknál foglalnak helyet,
  így kihagyhatatlan, hogy legyen az asztalokról nyilvántartásunk.

Fontos, hogy az adatmodell készítésekor az alapvető érvényességi feltételekre is koncentráljunk,
később mindig nehezebb beépíteni!
Fontos, hogy a szöveges mezők (ha nincs rajtuk egyéb megszorítás), akkor tetszöleges hosszú szöveget tartalmazhatnak (nvarchar(max)).

- Tegyünk fel olyan elemeket az étlapra, amikre nem készültünk fel:
  - Például szeretnénk desszertet felvenni az étlapra, és legyen cukormentes is.
  - Az italok közül vannak szénsavas és szénsavmentes is.
  - Az ételeknél érdemes nyilvántartani, hogy tartalmaz-e húst.

- Jogosultságcsoportok és felhasználók feltése Seed-del.

## Tennivalók
### Adatok betöltése és kiírása a TablesController-en
- Az asztalok listáját szeretném úgy átszervezni, hogy külön csoportba kerüljön a kültéri és a beltéri asztalok listája
- Ehhez vagy átalakítom a nézet kódját, és feldolgozom valamint átalakítom a megkapott adatokat,
  vagy a vezérlőben dolgozom többet: előre feldolgozom az asztalok listáját és előállítok egy olyan modellt, amit könnyű megjeleníteni.
- Kell egy új ViewModel, ami tartalmazza a termek listáját, maj ezen belül az egyes termekben tartalmazza a teremben lévő asztalok listáját.
- A nézetnek az új ViewModel-t kell megjelenítenie.
- Az adatbázis modellemet is át tudom alakítani, hogy eleve megmondja a termekhez tartozó asztalok listáját
- Lenyílómező a Location-höz

## Jogosultságcsoportok és felhasználók feltöltése Seed-del
- Szerepek és felhasználók induló feltöltése
- A szerepeket at "Authorize" annotációval fixen beírjuk
- A felhasználók adminisztrációjának felületét ebből a kódból meg lehet írni, ha az alkalmazásban beépített szerepek vannak
  és van egy beépített adminisztrátor 

## Házi feladat
- az adatfeltöltéssel játszani (Migrations/Configuration/Seed), több asztal, több helyszín
- az Asztal osztálynak konstruktor készítése
- a Location osztálynak generált vezérlők és nézetek átnézése
- a Location és a Categories nézetek továbbfejlesztése (EditorTempltaes, DisplayTemplates) készítése és használata
- a Category osztály megjelenítése magyarul 
- jogosultságok beállítása a locations-nek megfelelő módon a Categories-nál is
- jogosultságok beállítása az Asztalok (Table) osztályon (vezérlőn, nézeteken)
- Adatok betöltése és kiírása a TablesController-en

- Az asztal kültéri vagy beltéri-e
- A szerepek és/vagy a felhasználók adminisztrációjához saját webes felület létrehozása az itteni kódok alapján
 
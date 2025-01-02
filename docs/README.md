# Upravljanje opravil
Rešitev za implementacijo API-ja za upravljanje zahtevkov z uporabo ASP.NET Core in arhitekturnega vzorca Clean Arhitecture. Podpira testiranje in pakiranje v Docker kontejner.

## Zagon aplikacije
### Visual Studio 22
<a name="vs22anchor"></a>
1. Zaženite Visual Studio 22
2. Odprite rešitev **TestnaNalogaOresnik.sln** s klikom na **File > Open > Project/Solution**
3. Obnovite odvisnosti z desnim klikom na rešitev in izberite **Restore NuGet Packages**
4. Zaženite rešitev s klikom na gumb **Run** (zelena puščica)
5. Aplikacijo lahko testirate preko vgrajenega orodja Swagger ali pa z zunanjimi orodji kot so npr. Postman (`http://localhost:5069/api/Task/tasks`)

### Docker
1. Namestite Docker Desktop
2. Odprite ukazno okno in se postavite v direktorij rešitve
3. Zaženite ukaz `docker-compose build`
4. Ko je slika zgrajena, jo zaženite v vmesniku Docker Desktop ali z ukazom `docker-compose up`
5. Do aplikacije lahko dostopate preko brskalnika ali zunanjih orodij kot so npr. Postman (`http://localhost:5000/api/Task/tasks`)
6. Kontejner zaustavite z ukazom `docker-compose down` ali preko vmesnika Docker Desktop

## Logiranje
Aplikacija beleži dogodke in napake v datoteki `logs.txt` (*\TestnaNalogaOresnik\logs.txt*)

## Testiranje
Rešitev vključuje projekt za tesitranje aplikacije **Test**. Testiranje lahko izvedemo na dva načina
### Visual Studio 22
1. [Kot v zagonu aplikacije](#vs22anchor), odprite projekt v Visual Studio 22
2. Najdite projekt **Test** in z desnim klikom odprite meni. Izberite **Run Tests**

### Ukazno okno
1. Odprite ukazno okno in se postavite v direktorij rešitve
2. Zaženite ukaz `dotnet test`

## Odpravljanje napak
- Preverite, da so v rešitvi obnovljene odvisnosti **Restore NuGet Packages**
- Preverite, da je .NET verzija nastavljena na 8

<br>

## Avtor
Leo Orešnik

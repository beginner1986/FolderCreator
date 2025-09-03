# Aplikacja Folderowa

## Opis

Aplikacja Folderowa to narzêdzie WPF, które umo¿liwia tworzenie struktury folderów na podstawie predefiniowanych szablonów. U¿ytkownik mo¿e definiowaæ szablony folderów, edytowaæ je oraz u¿ywaæ do szybkiego tworzenia zestawów folderów w wybranej lokalizacji.

## Funkcje

*   **Szablony folderów:** Definiowanie hierarchicznych szablonów folderów z mo¿liwoœci¹ dodawania podfolderów.
*   **Edycja szablonów:** Mo¿liwoœæ modyfikacji istniej¹cych szablonów folderów.
*   **Wyszukiwanie:** Wyszukiwanie szablonów po nazwie i zawartoœci.
*   **U¿ywanie szablonów:** Szybkie tworzenie struktury folderów na podstawie szablonu.
*   **Interfejs u¿ytkownika:** Intuicyjny interfejs WPF.
*   **Skróty klawiszowe:** Obs³uga skrótów klawiszowych dla szybszej pracy (Ctrl+N - Nowy szablon, Ctrl+E - Edytuj szablon, etc.).

## Wymagania

*   .NET 8 Runtime

## Instrukcja budowania release z linii poleceñ

### Budowanie instalatora MSI

Aby zbudowaæ instalator MSI aplikacji:
1. Otwórz PowerShell lub Windows Terminal.
2. PrzejdŸ do katalogu g³ównego projektu.
3. Uruchom skrypt build.bat (wymaga PowerShell):
    ```powershell
    .\build.bat
    ```
4. Po zakoñczeniu procesu, instalator MSI zostanie utworzony w folderze `build`.

### Manualne budowanie aplikacji (bez instalatora)

Aby rêcznie zbudowaæ tylko aplikacjê .NET bez tworzenia instalatora:
1. Otwórz terminal lub wiersz poleceñ.
2. PrzejdŸ do katalogu `App`, gdzie znajduje siê plik projektu.
3. Wykonaj nastêpuj¹ce polecenie, aby zbudowaæ projekt w trybie Release:
    ```bash
    dotnet publish -c Release -o ./publish
    ```

## Jak u¿ywaæ

1.  Uruchom aplikacjê.
2.  Przejrzyj dostêpne szablony folderów w drzewie widoku.
3.  U¿yj przycisku "U¿yj szablonu", aby utworzyæ strukturê folderów na podstawie wybranego szablonu.
4.  Wybierz lokalizacjê, w której chcesz utworzyæ foldery.
5.  Opcjonalnie, wprowadŸ wartoœci zmiennych, jeœli szablon ich wymaga.
6.  Kliknij "OK", aby utworzyæ foldery.


## Autor

Adam Emieljaniuk

## Licencja

MIT
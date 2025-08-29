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

## Jak u¿ywaæ

1.  Uruchom aplikacjê.
2.  Przejrzyj dostêpne szablony folderów w drzewie widoku.
3.  U¿yj przycisku "U¿yj szablonu", aby utworzyæ strukturê folderów na podstawie wybranego szablonu.
4.  Wybierz lokalizacjê, w której chcesz utworzyæ foldery.
5.  Opcjonalnie, wprowadŸ wartoœci zmiennych, jeœli szablon ich wymaga.
6.  Kliknij "OK", aby utworzyæ foldery.

## Instrukcja budowania release z linii poleceñ
Aby zbudowaæ wersjê Release aplikacji z linii poleceñ, wykonaj nastêpuj¹ce kroki:
1.  Otwórz terminal lub wiersz poleceñ.
2. PrzejdŸ do katalogu g³ównego projektu, gdzie znajduje siê plik `FolderCreator.csproj`.
3. Wykonaj nastêpuj¹ce polecenie, aby zbudowaæ projekt w trybie Release i opublikowaæ go do okreœlonego folderu:
    ```bash
    dotnet publish -c Release -o ./publish
    ```
1. 
## Autor

Adam Emieljaniuk

## Licencja

MIT
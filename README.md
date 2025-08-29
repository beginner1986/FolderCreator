# Aplikacja Folderowa

## Opis

Aplikacja Folderowa to narz�dzie WPF, kt�re umo�liwia tworzenie struktury folder�w na podstawie predefiniowanych szablon�w. U�ytkownik mo�e definiowa� szablony folder�w, edytowa� je oraz u�ywa� do szybkiego tworzenia zestaw�w folder�w w wybranej lokalizacji.

## Funkcje

*   **Szablony folder�w:** Definiowanie hierarchicznych szablon�w folder�w z mo�liwo�ci� dodawania podfolder�w.
*   **Edycja szablon�w:** Mo�liwo�� modyfikacji istniej�cych szablon�w folder�w.
*   **Wyszukiwanie:** Wyszukiwanie szablon�w po nazwie i zawarto�ci.
*   **U�ywanie szablon�w:** Szybkie tworzenie struktury folder�w na podstawie szablonu.
*   **Interfejs u�ytkownika:** Intuicyjny interfejs WPF.
*   **Skr�ty klawiszowe:** Obs�uga skr�t�w klawiszowych dla szybszej pracy (Ctrl+N - Nowy szablon, Ctrl+E - Edytuj szablon, etc.).

## Wymagania

*   .NET 8 Runtime

## Jak u�ywa�

1.  Uruchom aplikacj�.
2.  Przejrzyj dost�pne szablony folder�w w drzewie widoku.
3.  U�yj przycisku "U�yj szablonu", aby utworzy� struktur� folder�w na podstawie wybranego szablonu.
4.  Wybierz lokalizacj�, w kt�rej chcesz utworzy� foldery.
5.  Opcjonalnie, wprowad� warto�ci zmiennych, je�li szablon ich wymaga.
6.  Kliknij "OK", aby utworzy� foldery.

## Instrukcja budowania release z linii polece�
Aby zbudowa� wersj� Release aplikacji z linii polece�, wykonaj nast�puj�ce kroki:
1.  Otw�rz terminal lub wiersz polece�.
2. Przejd� do katalogu g��wnego projektu, gdzie znajduje si� plik `FolderCreator.csproj`.
3. Wykonaj nast�puj�ce polecenie, aby zbudowa� projekt w trybie Release i opublikowa� go do okre�lonego folderu:
    ```bash
    dotnet publish -c Release -o ./publish
    ```
1. 
## Autor

Adam Emieljaniuk

## Licencja

MIT
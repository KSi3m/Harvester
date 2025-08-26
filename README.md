# Harvester

**Harvester** to aplikacja do zarządzania zamówieniami kombajnów do koszenia zboża. Umożliwia tworzenie ofert, dodawanie maszyn do systemu oraz planowanie pracy na polach. Projekt jest rozwijany po godzinach i obecnie zbliża się do wydania wersji **0.1.0** (połowa października).  

## Funkcjonalności w wersji 0.1.0
- Dodawanie pól uprawnych.
  - Pobranie danych dot. powierzchni pola na podstawie numeru identyfikacyjnego działki z zewnętrznego API
- Dodawanie kombajnów do systemu.
- Generowanie zamówień na kombajn.

## Planowane funkcjonalności w kolejnych wersjach
- System logowania dla różnych ról:
  - Kombajnista
  - Użytkownik zwykły  
- Zarządzanie flotą kombajnów: przypisywanie maszyn do konkretnych zleceń i harmonogramów.
- Historia realizacji zamówień i raporty dla klientów.
- Panel administracyjny do zarządzania użytkownikami i ofertami.
- Automatyczne ustalanie wskaźników pól dot. jego pochyłowatości/pofalowania i kształtu pola na podstawie danych z Geoportalu (obecnie owe wskaźniki podawane są przez użytkownika z listy rozwijanej)
- Integracja z lokalizacją GPS dla kombajnów.
- Powiadomienia o statusie realizacji zamówień.
- Analiza efektywności pracy kombajnów na podstawie wskaźników pól i danych historycznych.

## Technologie
Projekt wykorzystuje stack:
- **Backend:** .NET 8, Entity Framework 8
- **Frontend:** Angular 20, PrimeNG
- **Baza danych:** SQL Server

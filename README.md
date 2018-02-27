# Reprezentacja wiedzy

## Projekt nr 4: **Programy działań z akcjami współbieżnymi**

Dana jest klasa systemów dynamicznych spełniających następujące warunki:

1. Prawo inercji.
2. Niedeterminizm.
3. W języku kwerend występują akcje złożone (zbiory co najwyżej *k* akcji atomowych), w języku akcji jedynie akcje atomowe.
4. Pełna informacja o wszystkich akcjach atomowych i wszystkich ich skutkach bezpośrednich.
5. Z każdą akcją atomową związany jest jej warunek wstępny (ew. `TRUE`) i końcowy (efekt akcji).
6. Wykonywane są jedynie akcje bezkonfliktowe (żadne dwie akcje składowe nie mogą mieć wspólnych zmiennych, na które w żadnym stanie mają wpływ).
7. Wynikiem akcji złożonej jest suma skutków wszystkich składowych akcji bezkonfliktowych.
8. Akcje mogą być niewykonalne w pewnych stanach; jeśli akcja jest niewykonywalna, to każda akcja ją zawierająca też jest niewykonalna.
9. Dopuszczalny jest opis częściowy zarówno stanu początkowego, jak i pewnych stanów wynikających z wykonań sekwencji akcji.

*Programem P działań* jest ciąg *P = (A<sub>1</sub>,...,A<sub>n</sub>)* akcji złożonych.
Jest on *realizowalny*, jeśli wszystkie akcje złożone *A<sub>i</sub>* są wykonalne.

Celem projektu jest opracowanie i zaimplementowanie języka akcji dla specyfikacji podanej klasy systemów dynamicznych oraz odpowiadającego mu języka kwerend zapewniającego uzyskanie odpowiedzi na następujące zapytania:

1. Czy podany program *P* działań jest możliwy do realizacji zawsze/kiedykolwiek ze stanu początkowego?
2. Czy wykonanie programu *P* w stanie początkowym działań prowadzi zawsze/kiedykolwiek do osiągnięcia celu *γ*?
3. Czy cel *γ* jest osiągalny ze stanu początkowego?
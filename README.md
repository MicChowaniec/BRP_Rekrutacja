Zadanie wykonane.
-Kontrola GamePadem: Lewy Joystick do nawigacji + SouthButton dla klikania przycisków.
-Zmieniłem kolory w Selected dla niektórych obiektów dla lepszej widczności podświetlonych elementów. W niektórych miejscach wskazanie myszą i zaznaczenie ma ten sam efekt/kolor, co może być mylące, dlatego zalecam używanie jednego rodzaju kontrolera w tej wersji gry.
-W przeciwieństwie do grania myszą, przy grze padem nie da się walczyć jednocześnie z kilkoma przeciwnikami.
-Wychodzenie z ekranu poprzez wciśnięcie przycisku X dla ekwipunku, lub resume dla options.
-Build w folderze o tytule Build z datą.
-Niektóre potwory są podatne na jeden rodzaj broni. Standardowo za użycie Soul lub pokonanie potwora jest 10punktów, za pokonanie potwora przy użyciu konkretnej broni +5 punktów. Punkty wyświetlają się na środku pierwszego ekranu na samej górze.
-Do prawidłowej nawigacji po przyciskach użyłem Stack<List<GameObject>> który przechowuje referencje do przycisków z konkretnych paneli. Podczas załadowania nowego panelu, panel jest skanowany pod kątem Selectabli w Children, a następnie dodawana jest nowa warstwa do stosu. Pozostałe Selectable w innych warstwach stają się nieaktywne, a po zamknięciu panelu, stos usuwa górną warstwę, a aktywna staje się grupa przycisków z wartswy poniżej.
-Dodałem również blokowanie przycisku USE tam gdzie nie jest możliwe jego użycie.

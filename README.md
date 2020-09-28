# Projekt-Telefonia-IP

## Charakterystyka ogólna projektu
Projekt ma być aplikacją służącą do komunikowania się z innym użytkownikiem przez internet zarówno za pomocą tekstu jak i głosu. Główne założenia obejmują wysokie bezpieczeństwo oraz prywatność konwersacji. System składa się z aplikacji serwerowej służącej do ustalania sesji między aplikacjami klienckimi i przekazywania wiadomości oraz aplikacji klienckiej wyposażonej w interfejs graficzny umożliwiający zarówno rozpoczęcie połączenia głosowego, jak i czytanie lub wysyłanie wiadomości tekstowych. Po ustaleniu połączenia przesył głosu odbywa się bezpośrednio pomiędzy aplikacjami klienckimi.

## Architektura systemu
Komunikacja odbywa się pomiędzy klientem a serwerem za pomocą 25 komunikatów protokołu własnego aplikacji składającego się z nagłówka i zmiennego pola danych w zależności od komunikatu. Służą one do obsługi zadań takich jak na przykład: logowanie się i rejestracja, dodawanie kontaktów i wysyłanie im wiadomości oraz inicjalizacja połączenia głosowego. Dostarczane są one za pomocą protokołu warstwy transportowej TCP. Sesja między klientem a serwerem jest ustalana za pomocą algorytmu wymiany kluczy Diffiego-Hellmana, gdzie przy generowaniu kluczy publicznych i prywatnych zastosowany został algorytm haszujący SHA-256. Po ustaleniu klucza sesji algorytmem wykorzystany do szyfrowania komunikatów jest AES. Po zaszyfrowaniu wiadomość jest jeszcze zakodowywana Base64 w celu zapewnienia większej elastyczności w razie dalszych usprawnień systemu. Połączenie głosowe odbywa się bezpośrednio pomiędzy klientami. Dźwięk jest przesyłany datagramami UDP w formie PCM o częstotliwości próbkowania 22 KHz przy 16 bitach na próbkę i reprezentowany przez dwa kanały. Wiadomości, listy znajomych oraz blokowanych użytkowników i historia połączeń są przechowywane w bazie danych serwera korzystającej z SQLite i ADO.NET Entity Framework do opisu struktury danych.

## Narzędzia, środowisko, biblioteki, kodeki
C#, .NET Framework, WindowsForms, ADO.NET Entity Framework, NAudio, SQLite, <br />
Protokoły: UDP, TCP, <br />
Format dźwięku: PCM (22Khz, 16bit, 2 canals), <br />
Bezpieczeństwo: Diffi-Hellman, SHA-256, AES.

Detta projekt är ett system för att kunna skapa och räkna ut en budget för en användare.<br />
Efter inmatning utav budgetuppgifter så görs grafer och presentationer över vart pengarna går och vilka poster som sticker ut. .<br />
Byggt i C#.net, använder WPF och allt lagras i en MySql databas.<br />
<br />
Projektet innhåller följande:<br />
<br />
[x] En databas (MySQL server) med tillhörande tabeller<br />
-Det finns en komplett .SQL-fil i projektet som skapar databas + tabeller och en användare.  <br />
<br />
[x] Ett registeringsformulär<br />
-Användare kan registera sig och skapa en fungerande användare. <br />
<br />
[x] Ett inloggningsfönster<br />
-Användare kan sedan logga in med användaren på ett säkert sätt. Stöd för epost verifiering finns med i tankarna. <br />
<br />
[x] Ett inledande formulär om uppsättning av budget<br />
-Vid första inloggning får användare mata in uppgifter om sin budget och eventuellt om den har en delad eknomi, uppgifterna sparas sedan i databasen. <br />
<br />
[/] Snygga grafer<br />
<br />
[ ] Bra budgettips<br />
<br />
Stack: <br />
Backend<br />
-MySQL Server för databaslagring. <br />
<br />
Frontend<br />
-C# med Windows Presentation Foundation (WPF) som utnyttjar .NET-ramverkets möjligheter att bygga bra robust kod. <br />
<br />
Arkitektur-mönster<br />
-Model-View-ViewModel (MVVM) mönster, säkerställer en ren separation av ansvarsområden.

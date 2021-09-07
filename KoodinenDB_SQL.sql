CREATE DATABASE KoodinenDB;

USE KoodinenDB;

CREATE TABLE Kayttaja (
kayttaja_id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
nimi VARCHAR(100) NULL,
email VARCHAR(100) NOT NULL,
salasana VARCHAR(50) NOT NULL
);

CREATE TABLE Kurssi (
kurssi_id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
nimi VARCHAR(100) NULL,
kuvaus VARCHAR(400) NULL,
kayttaja_id INT FOREIGN KEY REFERENCES Kayttaja(kayttaja_id)
);

CREATE TABLE KurssiSuoritus (
kurssiSuoritus_id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
suoritusPvm DATE NULL,
kayttaja_id INT FOREIGN KEY REFERENCES Kayttaja(kayttaja_id),
kurssi_id INT FOREIGN KEY REFERENCES Kurssi(kurssi_id)
);

CREATE TABLE Oppitunti (
oppitunti_id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
nimi VARCHAR(100) NULL,
kuvaus VARCHAR(400) NULL,
kurssi_id INT FOREIGN KEY REFERENCES Kurssi(kurssi_id)
);

CREATE TABLE OppituntiSuoritus (
oppituntiSuoritus_id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
suoritusPvm DATE NULL,
kayttaja_id INT FOREIGN KEY REFERENCES Kayttaja(kayttaja_id),
oppitunti_id INT FOREIGN KEY REFERENCES Oppitunti(oppitunti_id)
);

CREATE TABLE Tehtava (
tehtava_id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
nimi VARCHAR(200) NULL,
kuvaus VARCHAR(400) NULL,
vihje VARCHAR(700) NULL,
tehtavaUrl VARCHAR(200) NULL,
oppitunti_id INT FOREIGN KEY REFERENCES Oppitunti(oppitunti_id)
);

CREATE TABLE TehtavaSuoritus (
TehtavaSuoritus_id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
suoritusPvm DATE NULL,
kayttaja_id INT FOREIGN KEY REFERENCES Kayttaja(kayttaja_id),
tehtava_id INT FOREIGN KEY REFERENCES Tehtava(tehtava_id)
);


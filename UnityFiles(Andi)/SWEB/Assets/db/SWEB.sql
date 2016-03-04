--
-- File generated with SQLiteStudio v3.0.7 on Fr. Mär 4 02:20:13 2016
--
-- Text encoding used: windows-1252
--
PRAGMA foreign_keys = off;
BEGIN TRANSACTION;

-- Table: effecType
CREATE TABLE effecType (id INTEGER PRIMARY KEY AUTOINCREMENT, effect CHAR)
INSERT INTO effecType (id, effect) VALUES (1, 'lv_counter');
INSERT INTO effecType (id, effect) VALUES (2, 'swap_card');
INSERT INTO effecType (id, effect) VALUES (3, 'destroy_lv_card');
INSERT INTO effecType (id, effect) VALUES (4, 'get_card_from_deck');
INSERT INTO effecType (id, effect) VALUES (5, 'modify_attack');
INSERT INTO effecType (id, effect) VALUES (6, 'modify_defense');
INSERT INTO effecType (id, effect) VALUES (7, 'modify_ects');
INSERT INTO effecType (id, effect) VALUES (8, 'modify_defense_all');
INSERT INTO effecType (id, effect) VALUES (9, 'switch_attack_defense');
INSERT INTO effecType (id, effect) VALUES (10, 'encrypt');
INSERT INTO effecType (id, effect) VALUES (11, 'none');

-- Table: cardReward
CREATE TABLE cardReward (id INTEGER PRIMARY KEY AUTOINCREMENT, sourceCard INTEGER REFERENCES card (id), rewardCard INTEGER REFERENCES card (id))
INSERT INTO cardReward (id, sourceCard, rewardCard) VALUES (1, 3, 1);
INSERT INTO cardReward (id, sourceCard, rewardCard) VALUES (2, 3, 43);
INSERT INTO cardReward (id, sourceCard, rewardCard) VALUES (3, 14, 31);
INSERT INTO cardReward (id, sourceCard, rewardCard) VALUES (4, 14, 44);
INSERT INTO cardReward (id, sourceCard, rewardCard) VALUES (5, 15, 28);
INSERT INTO cardReward (id, sourceCard, rewardCard) VALUES (6, 15, 45);
INSERT INTO cardReward (id, sourceCard, rewardCard) VALUES (7, 16, 36);
INSERT INTO cardReward (id, sourceCard, rewardCard) VALUES (8, 16, 44);
INSERT INTO cardReward (id, sourceCard, rewardCard) VALUES (9, 17, 39);
INSERT INTO cardReward (id, sourceCard, rewardCard) VALUES (10, 17, 9);
INSERT INTO cardReward (id, sourceCard, rewardCard) VALUES (11, 18, 46);
INSERT INTO cardReward (id, sourceCard, rewardCard) VALUES (13, 19, 30);
INSERT INTO cardReward (id, sourceCard, rewardCard) VALUES (14, 19, 44);
INSERT INTO cardReward (id, sourceCard, rewardCard) VALUES (15, 20, 42);
INSERT INTO cardReward (id, sourceCard, rewardCard) VALUES (16, 20, 47);
INSERT INTO cardReward (id, sourceCard, rewardCard) VALUES (17, 21, 38);
INSERT INTO cardReward (id, sourceCard, rewardCard) VALUES (18, 21, 48);
INSERT INTO cardReward (id, sourceCard, rewardCard) VALUES (19, 22, 29);
INSERT INTO cardReward (id, sourceCard, rewardCard) VALUES (20, 22, 49);
INSERT INTO cardReward (id, sourceCard, rewardCard) VALUES (21, 23, 37);
INSERT INTO cardReward (id, sourceCard, rewardCard) VALUES (22, 23, 32);
INSERT INTO cardReward (id, sourceCard, rewardCard) VALUES (23, 24, 33);
INSERT INTO cardReward (id, sourceCard, rewardCard) VALUES (24, 24, 8);
INSERT INTO cardReward (id, sourceCard, rewardCard) VALUES (25, 25, 41);
INSERT INTO cardReward (id, sourceCard, rewardCard) VALUES (26, 25, 44);
INSERT INTO cardReward (id, sourceCard, rewardCard) VALUES (27, 26, 50);
INSERT INTO cardReward (id, sourceCard, rewardCard) VALUES (28, 26, 35);
INSERT INTO cardReward (id, sourceCard, rewardCard) VALUES (29, 27, 34);
INSERT INTO cardReward (id, sourceCard, rewardCard) VALUES (30, 27, 51);
INSERT INTO cardReward (id, sourceCard, rewardCard) VALUES (31, 4, 2);
INSERT INTO cardReward (id, sourceCard, rewardCard) VALUES (32, 4, 44);

-- Table: actioncard
CREATE TABLE actioncard (id INTEGER PRIMARY KEY REFERENCES card (id), description CHAR, effect INTEGER REFERENCES effect (id), cost INTEGER)
INSERT INTO actioncard (id, description, effect, cost) VALUES (5, 'Verlängert eine LV-Card um eine Runde', 2, -3);
INSERT INTO actioncard (id, description, effect, cost) VALUES (6, 'Verringert die Rundendauer einer LV-Card um 2', 1, -3);
INSERT INTO actioncard (id, description, effect, cost) VALUES (7, 'Vertausche einer deiner Karten mit der deines Gegners', 3, -5);
INSERT INTO actioncard (id, description, effect, cost) VALUES (8, 'Verringert die Attacke einer Wissenskarte um 2', 4, -3);
INSERT INTO actioncard (id, description, effect, cost) VALUES (9, 'Verringert die Rundendauer einer LV-Card um 1', 5, -1);
INSERT INTO actioncard (id, description, effect, cost) VALUES (10, 'Verringert die Rundendauer einer LV-Card um 2', 1, -3);
INSERT INTO actioncard (id, description, effect, cost) VALUES (11, 'Zerstöre eine LV-Karte', 6, -5);
INSERT INTO actioncard (id, description, effect, cost) VALUES (12, 'Ziehe eine zusätzliche Karte', 7, -2);
INSERT INTO actioncard (id, description, effect, cost) VALUES (13, 'Verlängert eine LV-Card um zwei  Runden', 8, -3);
INSERT INTO actioncard (id, description, effect, cost) VALUES (43, 'Du erhältst +1 ECTS', 10, 0);
INSERT INTO actioncard (id, description, effect, cost) VALUES (44, 'Du erhältst +2 ECTS', 11, 0);
INSERT INTO actioncard (id, description, effect, cost) VALUES (45, 'Verringere die Verteidigun aller feindlicher Wissenskarten um 1', 12, 0);
INSERT INTO actioncard (id, description, effect, cost) VALUES (46, 'Du erhältst +4 ECTS', 13, 0);
INSERT INTO actioncard (id, description, effect, cost) VALUES (47, 'Du erhältst +3 ECTS', 14, 0);
INSERT INTO actioncard (id, description, effect, cost) VALUES (48, 'Vertausche Angriffs- und Verteidigungswert', 15, 0);
INSERT INTO actioncard (id, description, effect, cost) VALUES (49, 'Verringere die Verteidigung einer feindlichen Wissenskarte um 4', 16, 0);
INSERT INTO actioncard (id, description, effect, cost) VALUES (50, 'Verschlüssle eine Karte', 17, 0);
INSERT INTO actioncard (id, description, effect, cost) VALUES (51, 'Du erhältst +7 ECTS', 18, 0);

-- Table: effect
CREATE TABLE effect (id INTEGER PRIMARY KEY AUTOINCREMENT, effectType INTEGER REFERENCES effecType (id), effectValue INTEGER)
INSERT INTO effect (id, effectType, effectValue) VALUES (1, 1, -2);
INSERT INTO effect (id, effectType, effectValue) VALUES (2, 1, 1);
INSERT INTO effect (id, effectType, effectValue) VALUES (3, 2, 1);
INSERT INTO effect (id, effectType, effectValue) VALUES (4, 5, -2);
INSERT INTO effect (id, effectType, effectValue) VALUES (5, 1, -1);
INSERT INTO effect (id, effectType, effectValue) VALUES (6, 3, 1);
INSERT INTO effect (id, effectType, effectValue) VALUES (7, 4, 1);
INSERT INTO effect (id, effectType, effectValue) VALUES (8, 1, 2);
INSERT INTO effect (id, effectType, effectValue) VALUES (9, 6, -2);
INSERT INTO effect (id, effectType, effectValue) VALUES (10, 7, 1);
INSERT INTO effect (id, effectType, effectValue) VALUES (11, 7, 2);
INSERT INTO effect (id, effectType, effectValue) VALUES (12, 8, 1);
INSERT INTO effect (id, effectType, effectValue) VALUES (13, 7, 4);
INSERT INTO effect (id, effectType, effectValue) VALUES (14, 7, 3);
INSERT INTO effect (id, effectType, effectValue) VALUES (15, 9, 1);
INSERT INTO effect (id, effectType, effectValue) VALUES (16, 6, -4);
INSERT INTO effect (id, effectType, effectValue) VALUES (17, 10, 1);
INSERT INTO effect (id, effectType, effectValue) VALUES (18, 7, 7);
INSERT INTO effect (id, effectType, effectValue) VALUES (19, 11, 0);

-- Table: knowledgecard
CREATE TABLE knowledgecard (id INTEGER PRIMARY KEY REFERENCES card (id), effect INTEGER REFERENCES effect (id), attack INTEGER, defense INTEGER)
INSERT INTO knowledgecard (id, effect, attack, defense) VALUES (1, 19, 1, 2);
INSERT INTO knowledgecard (id, effect, attack, defense) VALUES (2, 19, 2, 1);
INSERT INTO knowledgecard (id, effect, attack, defense) VALUES (28, 19, 3, 2);
INSERT INTO knowledgecard (id, effect, attack, defense) VALUES (29, 9, 2, 2);
INSERT INTO knowledgecard (id, effect, attack, defense) VALUES (30, 19, 2, 1);
INSERT INTO knowledgecard (id, effect, attack, defense) VALUES (31, 19, 2, 1);
INSERT INTO knowledgecard (id, effect, attack, defense) VALUES (32, 19, 1, 2);
INSERT INTO knowledgecard (id, effect, attack, defense) VALUES (33, 19, 3, 1);
INSERT INTO knowledgecard (id, effect, attack, defense) VALUES (34, 19, 6, 4);
INSERT INTO knowledgecard (id, effect, attack, defense) VALUES (35, 19, 3, 1);
INSERT INTO knowledgecard (id, effect, attack, defense) VALUES (36, 19, 2, 1);
INSERT INTO knowledgecard (id, effect, attack, defense) VALUES (37, 19, 2, 1);
INSERT INTO knowledgecard (id, effect, attack, defense) VALUES (38, 19, 2, 2);
INSERT INTO knowledgecard (id, effect, attack, defense) VALUES (39, 19, 3, 2);
INSERT INTO knowledgecard (id, effect, attack, defense) VALUES (40, 19, 1, 5);
INSERT INTO knowledgecard (id, effect, attack, defense) VALUES (41, 19, 3, 3);
INSERT INTO knowledgecard (id, effect, attack, defense) VALUES (42, 19, 2, 2);

-- Table: card
CREATE TABLE card (id INTEGER PRIMARY KEY AUTOINCREMENT, name CHAR UNIQUE, image CHAR, cardType INTEGER)
INSERT INTO card (id, name, image, cardType) VALUES (1, 'Turing Maschine', 'cardfaces/c15', 3);
INSERT INTO card (id, name, image, cardType) VALUES (2, 'Cookie Monster', 'cardfaces/c19', 3);
INSERT INTO card (id, name, image, cardType) VALUES (3, 'Grundlagen der Informatik', 'cardfaces/c15-cardfaces/ects', 2);
INSERT INTO card (id, name, image, cardType) VALUES (4, 'Internet und neue Medien', 'cardfaces/c19-cardfaces/ects', 2);
INSERT INTO card (id, name, image, cardType) VALUES (5, 'Friendzone', 'cardfaces/c10', 1);
INSERT INTO card (id, name, image, cardType) VALUES (6, 'Nachhilfe', 'cardfaces/c9', 1);
INSERT INTO card (id, name, image, cardType) VALUES (7, 'Substitution', 'cardfaces/c6', 1);
INSERT INTO card (id, name, image, cardType) VALUES (8, 'Dämpfung', 'cardfaces/c7', 1);
INSERT INTO card (id, name, image, cardType) VALUES (9, 'Cache L1', 'cardfaces/c8', 1);
INSERT INTO card (id, name, image, cardType) VALUES (10, 'Schummelzettel', 'cardfaces/c4', 1);
INSERT INTO card (id, name, image, cardType) VALUES (11, 'Asymptote', 'cardfaces/c5', 1);
INSERT INTO card (id, name, image, cardType) VALUES (12, 'Wahlfach', 'cardfaces/c1', 1);
INSERT INTO card (id, name, image, cardType) VALUES (13, 'Saufen', 'cardfaces/c3', 1);
INSERT INTO card (id, name, image, cardType) VALUES (14, 'Analysis 1', 'cardfaces/c14-cardfaces/ects', 2);
INSERT INTO card (id, name, image, cardType) VALUES (15, 'Computer Graphics 1', 'cardfaces/c28-cardfaces/c31', 2);
INSERT INTO card (id, name, image, cardType) VALUES (16, 'Technische Informatik 1', 'cardfaces/c26-cardfaces/ects', 2);
INSERT INTO card (id, name, image, cardType) VALUES (17, 'Rechnerorganisation', 'cardfaces/c25-cardfaces/c8', 2);
INSERT INTO card (id, name, image, cardType) VALUES (18, 'Messtechnik 1', 'cardfaces/ects-cardfaces/c20', 2);
INSERT INTO card (id, name, image, cardType) VALUES (19, 'Einführung in die strukturierte Programmierung', 'cardfaces/c18-cardfaces/ects', 2);
INSERT INTO card (id, name, image, cardType) VALUES (20, 'Softwareentwicklung Praktikum', 'cardfaces/c22-cardfaces/ects', 2);
INSERT INTO card (id, name, image, cardType) VALUES (21, 'Datenstrukturen und Algorithmen', 'cardfaces/c24-cardfaces/c29', 2);
INSERT INTO card (id, name, image, cardType) VALUES (22, 'Computer Graphics 2', 'cardfaces/c17-cardfaces/c32', 2);
INSERT INTO card (id, name, image, cardType) VALUES (23, 'Grundlagen der Elektrotechnik', 'cardfaces/c23-cardfaces/c16', 2);
INSERT INTO card (id, name, image, cardType) VALUES (24, 'Regelungstechnik', 'cardfaces/c11-cardfaces/c7', 2);
INSERT INTO card (id, name, image, cardType) VALUES (25, 'Systemnahe Programmierung', 'cardfaces/c21-cardfaces/ects', 2);
INSERT INTO card (id, name, image, cardType) VALUES (26, 'Einfürhung in die Informationssicherheit', 'cardfaces/c30-cardfaces/c27', 2);
INSERT INTO card (id, name, image, cardType) VALUES (27, 'Betriebssysteme', 'cardfaces/c12-cardfaces/ects', 2);
INSERT INTO card (id, name, image, cardType) VALUES (28, 'Polygon', 'cardfaces/c28', 3);
INSERT INTO card (id, name, image, cardType) VALUES (29, 'Wireframe', 'cardfaces/c17', 3);
INSERT INTO card (id, name, image, cardType) VALUES (30, 'Nullpointer Exception', 'cardfaces/c18', 3);
INSERT INTO card (id, name, image, cardType) VALUES (31, 'Kampfintegral', 'cardfaces/c14', 3);
INSERT INTO card (id, name, image, cardType) VALUES (32, 'Elektronische Feldkartoffel', 'cardfaces/c16', 3);
INSERT INTO card (id, name, image, cardType) VALUES (33, 'Instabieler Regelkreis', 'cardfaces/c11', 3);
INSERT INTO card (id, name, image, cardType) VALUES (34, 'SWEB', 'cardfaces/c12', 3);
INSERT INTO card (id, name, image, cardType) VALUES (35, 'Brute force Attack', 'cardfaces/c27', 3);
INSERT INTO card (id, name, image, cardType) VALUES (36, 'Python', 'cardfaces/c26', 3);
INSERT INTO card (id, name, image, cardType) VALUES (37, 'Stromvektor', 'cardfaces/c23', 3);
INSERT INTO card (id, name, image, cardType) VALUES (38, 'Halde', 'cardfaces/c24', 3);
INSERT INTO card (id, name, image, cardType) VALUES (39, 'RS Flipflop', 'cardfaces/c25', 3);
INSERT INTO card (id, name, image, cardType) VALUES (40, 'Guardian Ring', 'cardfaces/c20', 3);
INSERT INTO card (id, name, image, cardType) VALUES (41, 'Deadlock', 'cardfaces/c21', 3);
INSERT INTO card (id, name, image, cardType) VALUES (42, 'Coding Standard', 'cardfaces/c22', 3);
INSERT INTO card (id, name, image, cardType) VALUES (43, 'ECTS +1', 'cardfaces/ects', 1);
INSERT INTO card (id, name, image, cardType) VALUES (44, 'ECTS +2', 'cardfaces/ects', 1);
INSERT INTO card (id, name, image, cardType) VALUES (45, 'Schwarz/Weiß - Shader', 'cardfaces/c31', 1);
INSERT INTO card (id, name, image, cardType) VALUES (46, 'ECTS +4', 'cardfaces/ects', 1);
INSERT INTO card (id, name, image, cardType) VALUES (47, 'ECTS +3', 'cardfaces/ects', 1);
INSERT INTO card (id, name, image, cardType) VALUES (48, 'Baum', 'cardfaces/c29', 1);
INSERT INTO card (id, name, image, cardType) VALUES (49, 'Grafik Glitch', 'cardfaces/c32', 1);
INSERT INTO card (id, name, image, cardType) VALUES (50, 'RSA', 'cardfaces/c30', 1);
INSERT INTO card (id, name, image, cardType) VALUES (51, 'ECTS +7', 'cardfaces/ects', 1);

-- Table: LVCard
CREATE TABLE LVCard (id INTEGER PRIMARY KEY REFERENCES card (id), startRound INTEGER, duration INTEGER, ECTSReward INTEGER)
INSERT INTO LVCard (id, startRound, duration, ECTSReward) VALUES (3, 1, 1, 1);
INSERT INTO LVCard (id, startRound, duration, ECTSReward) VALUES (4, 1, 1, 0);
INSERT INTO LVCard (id, startRound, duration, ECTSReward) VALUES (14, 1, 2, 1);
INSERT INTO LVCard (id, startRound, duration, ECTSReward) VALUES (15, 1, 1, 2);
INSERT INTO LVCard (id, startRound, duration, ECTSReward) VALUES (16, 2, 1, 2);
INSERT INTO LVCard (id, startRound, duration, ECTSReward) VALUES (17, 2, 2, 2);
INSERT INTO LVCard (id, startRound, duration, ECTSReward) VALUES (18, 3, 2, 2);
INSERT INTO LVCard (id, startRound, duration, ECTSReward) VALUES (19, 1, 2, 1);
INSERT INTO LVCard (id, startRound, duration, ECTSReward) VALUES (20, 3, 1, 2);
INSERT INTO LVCard (id, startRound, duration, ECTSReward) VALUES (21, 3, 1, 2);
INSERT INTO LVCard (id, startRound, duration, ECTSReward) VALUES (22, 3, 2, 3);
INSERT INTO LVCard (id, startRound, duration, ECTSReward) VALUES (23, 1, 1, 2);
INSERT INTO LVCard (id, startRound, duration, ECTSReward) VALUES (24, 4, 1, 2);
INSERT INTO LVCard (id, startRound, duration, ECTSReward) VALUES (25, 4, 1, 2);
INSERT INTO LVCard (id, startRound, duration, ECTSReward) VALUES (26, 5, 1, 2);
INSERT INTO LVCard (id, startRound, duration, ECTSReward) VALUES (27, 5, 3, 3);


-- some statements for testing:
-- View: getLVCard
CREATE VIEW getLVCard AS SELECT card.id, card.name, card.image, LVCard.startRound, LVCard.duration, LVCard.ECTSReward, cardReward.rewardCard FROM card INNER JOIN LVCard ON card.id = LVCard.id INNER JOIN cardReward ON card.id = cardReward.sourceCard
WHERE card.id = 4

-- View: getActionCard
CREATE VIEW getActionCard AS SELECT card.id, card.name, actioncard.description, actioncard.cost, effecType.effect, effect.effectValue FROM card INNER JOIN actioncard ON card.id = actioncard.id INNER JOIN effect ON actioncard.effect = effect.id INNER JOIN effecType ON effecType.id = effect.effectType WHERE card.id = 6

-- View: getKnowledgeCard
CREATE VIEW getKnowledgeCard AS SELECT card.id, card.name, card.image, knowledgecard.attack, knowledgecard.defense, effecType.effect, effect.effectValue
FROM card INNER JOIN knowledgecard ON card.id = knowledgecard.id LEFT JOIN effect ON knowledgecard.effect = effect.id LEFT JOIN effecType ON effecType.id = effect.effectType WHERE card.id = 2

COMMIT TRANSACTION;
PRAGMA foreign_keys = on;

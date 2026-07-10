# ORBITA AI — Technical Specification

> Ce document définit les grands principes techniques auxquels toute implémentation future du logiciel doit se conformer. Aucun choix de technologie, langage, framework ou infrastructure n'est fixé ici : ces décisions relèvent d'étapes ultérieures, encadrées par les principes définis dans ce document.

## 1. Maintenabilité

Le logiciel doit pouvoir être compris, corrigé et fait évoluer par une personne qui n'a pas participé à sa conception initiale, sans effort disproportionné.

- Toute logique métier doit être documentée à l'endroit où elle est mise en œuvre.
- La complexité doit être justifiée par un besoin réel, jamais introduite par anticipation d'un besoin hypothétique.
- Une dette technique identifiée doit être documentée et priorisée, jamais ignorée silencieusement.

## 2. Modularité

Le logiciel doit être conçu comme un ensemble de modules fonctionnels aux responsabilités clairement délimitées, correspondant aux domaines définis dans [FUNCTIONAL_SPECIFICATION.md](FUNCTIONAL_SPECIFICATION.md).

- Chaque module doit pouvoir évoluer indépendamment des autres, tant que les points d'échange entre modules restent stables.
- Une fonctionnalité ne doit dépendre d'une autre que par une interface clairement définie, jamais par un accès direct et implicite à son fonctionnement interne.
- Un module doit pouvoir être testé, compris et fait évoluer isolément.

## 3. Évolutivité

Le logiciel doit être capable d'absorber la croissance de l'organisation cliente et du produit sans remise en cause de ses fondations.

- La croissance du volume de données ne doit jamais dégrader la capacité de l'utilisateur à obtenir une réponse en temps utile.
- L'ajout d'une nouvelle fonctionnalité ou d'un nouveau module ne doit pas nécessiter de reconstruire les modules existants.
- La structure du logiciel doit anticiper une croissance progressive et durable, cohérente avec la vision à plusieurs années du projet (cf. [PROJECT_VISION.md](PROJECT_VISION.md)).

## 4. Lisibilité

Le logiciel doit être écrit pour être lu, pas seulement pour être exécuté.

- La structure du logiciel doit refléter directement le vocabulaire métier défini dans [GLOSSARY.md](GLOSSARY.md), pour qu'un lecteur puisse relier ce qu'il lit à un concept métier connu.
- La cohérence de nommage et de structure prime sur la préférence individuelle (cf. [PROJECT_RULES.md](PROJECT_RULES.md)).
- Toute exception à un principe établi doit être justifiée explicitement.

## 5. Performance

Le logiciel doit rester réactif quelle que soit l'ampleur des données traitées.

- Le temps de réponse perçu par l'utilisateur est un critère de qualité au même titre que l'exactitude du résultat.
- Les traitements longs (imports volumineux, projections complexes) doivent être conçus pour ne jamais bloquer l'usage du reste de la plateforme.
- La performance doit être considérée dès la conception d'une fonctionnalité, non corrigée après coup.

## 6. Sécurité

Le logiciel manipule des données commerciales sensibles et doit être conçu en conséquence dès sa fondation.

- La sécurité est un principe de conception, jamais une étape ajoutée après le développement d'une fonctionnalité.
- Les principes détaillés (confidentialité, intégrité, journalisation, droits d'accès, authentification, gestion des données) sont définis dans [SECURITY_PRINCIPLES.md](SECURITY_PRINCIPLES.md) et s'appliquent à l'ensemble du logiciel sans exception.

## 7. Principe transversal

Tout choix technique futur doit pouvoir être justifié par au moins un de ces six principes. Un choix qui améliore un aspect (par exemple la performance) au détriment d'un autre (par exemple la lisibilité ou la sécurité) doit faire l'objet d'un arbitrage explicite et documenté, jamais d'une décision implicite.

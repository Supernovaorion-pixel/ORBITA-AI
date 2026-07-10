# ORBITA AI — Code Quality

> Ce document fixe les exigences mesurables de qualité de code, déclinant en critères concrets les principes définis dans [architecture/CODING_PRINCIPLES.md](../architecture/CODING_PRINCIPLES.md) et [architecture/NAMING_CONVENTIONS.md](../architecture/NAMING_CONVENTIONS.md).

## 1. Style

- Le style de code (formatage, organisation des fichiers) est appliqué automatiquement par l'outillage défini dans [DEVELOPMENT_ENVIRONMENT.md](DEVELOPMENT_ENVIRONMENT.md) §4, sans variation d'un contributeur à l'autre.
- Les conventions de nommage suivent strictement [architecture/NAMING_CONVENTIONS.md](../architecture/NAMING_CONVENTIONS.md), vérifiées par analyse statique (cf. [DEVELOPMENT_ENVIRONMENT.md](DEVELOPMENT_ENVIRONMENT.md) §5).

## 2. Documentation

- Toute interface publique d'un module (méthode exposée à d'autres modules, contrat d'événement) est documentée dans le code source, expliquant son rôle métier, jamais son fonctionnement interne déjà lisible dans son implémentation.
- Une règle métier non évidente à la seule lecture du code est documentée à l'endroit exact où elle est implémentée, expliquant le "pourquoi", conformément à [architecture/CODING_PRINCIPLES.md](../architecture/CODING_PRINCIPLES.md) §8.
- L'absence de documentation sur une interface publique constitue un motif de refus de fusion (cf. [GIT_WORKFLOW.md](GIT_WORKFLOW.md) §4).

## 3. Complexité maximale

- La complexité cyclomatique d'une fonction ou méthode ne doit jamais dépasser un seuil de **10** ; au-delà, la fonction doit être décomposée en sous-fonctions à responsabilité unique (cf. [architecture/CODING_PRINCIPLES.md](../architecture/CODING_PRINCIPLES.md) §5).
- La profondeur d'imbrication (conditions, boucles) ne doit jamais dépasser **3 niveaux** ; une imbrication plus profonde est reformulée par extraction de fonction ou inversion de condition.
- Une classe ne doit jamais dépasser un seuil raisonnable de responsabilités mesurable par le nombre de raisons de changement identifiables ; au-delà de ce seuil, la classe est scindée conformément au principe de responsabilité unique.

## 4. Duplication

- Un même fragment de logique métier ne doit jamais être dupliqué au-delà d'un seuil de similarité significatif entre deux emplacements du code ; toute duplication détectée au-delà de ce seuil doit être résolue par extraction d'une capacité partagée, conformément au principe DRY (cf. [architecture/CODING_PRINCIPLES.md](../architecture/CODING_PRINCIPLES.md) §3).
- La duplication est mesurée automatiquement à chaque vérification préalable à la fusion (cf. [GIT_WORKFLOW.md](GIT_WORKFLOW.md) §4) et fait l'objet d'un suivi dans le temps, jamais d'une tolérance croissante.

## 5. Couverture de tests

- Toute règle métier du Domaine (cf. [architecture/DOMAIN_MODEL.md](../architecture/DOMAIN_MODEL.md)) est couverte par au moins un test unitaire (cf. [TESTING_STRATEGY.md](TESTING_STRATEGY.md) §2).
- Un seuil minimal de couverture de code est vérifié à chaque proposition de changement ; une diminution de la couverture globale du projet constitue un motif de vigilance renforcée en revue, sans pour autant justifier l'écriture de tests superflus uniquement destinés à satisfaire un chiffre (la pertinence du test prime toujours sur le seul pourcentage).
- La couverture est mesurée séparément pour le Domaine (exigence la plus stricte, cœur de la valeur métier) et pour l'Infrastructure (exigence proportionnée à la criticité de l'intégration concernée).

## 6. Lisibilité

- Une fonction ou méthode ne doit jamais dépasser une longueur raisonnable de lecture sans défilement excessif ; au-delà, elle doit être décomposée.
- Le nom d'une fonction, variable ou classe doit permettre à un lecteur de comprendre son rôle sans avoir à en lire l'implémentation (cf. [architecture/NAMING_CONVENTIONS.md](../architecture/NAMING_CONVENTIONS.md)).
- Un commentaire qui explique *ce que fait* le code plutôt que *pourquoi* il le fait est considéré comme un signe que le code lui-même doit être clarifié, jamais comme une qualité à conserver (cf. [architecture/CODING_PRINCIPLES.md](../architecture/CODING_PRINCIPLES.md) §7).

## 7. Revue de code

- Aucun changement n'est fusionné sans revue par au moins un autre contributeur que son auteur (cf. [GIT_WORKFLOW.md](GIT_WORKFLOW.md) §4), quelle que soit la taille du changement.
- La revue de code vérifie la conformité aux principes de ce document autant que la correction fonctionnelle du changement proposé.

## 8. Principe transversal

Ces seuils sont des garde-fous, pas des objectifs en soi : un code qui respecte tous les seuils mais reste difficile à comprendre pour un nouveau contributeur n'est pas considéré comme un code de qualité. La lisibilité réelle, évaluée en revue humaine, prévaut toujours sur la seule conformité automatisée.

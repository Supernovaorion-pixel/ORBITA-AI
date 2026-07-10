# ORBITA AI — Security Principles

> Ce document définit les principes de sécurité que toute implémentation future du logiciel doit respecter. Aucune technologie ou mécanisme d'implémentation n'est spécifié ici.

## 1. Confidentialité

- Les données d'une organisation cliente ne sont accessibles qu'aux utilisateurs de cette organisation, selon leurs droits d'accès définis.
- Aucune donnée d'une organisation n'est jamais exposée, directement ou indirectement, à une autre organisation cliente.
- L'accès à une donnée est toujours restreint au minimum nécessaire au rôle de l'utilisateur qui la consulte (principe du moindre privilège).

## 2. Intégrité

- Toute donnée intégrée à la plateforme (cf. module **Import**, [FUNCTIONAL_SPECIFICATION.md](FUNCTIONAL_SPECIFICATION.md)) doit être contrôlée avant intégration, afin de garantir sa cohérence et son exactitude.
- Une donnée validée ne peut être modifiée que par un processus tracé ; aucune modification silencieuse n'est admise.
- Les analyses, projections et rapports produits par la plateforme doivent toujours pouvoir être retracés jusqu'à la donnée source dont ils dérivent.

## 3. Sauvegardes

- Les données de chaque organisation cliente doivent faire l'objet de sauvegardes régulières, permettant une restauration en cas de perte ou de corruption.
- La fréquence et la rétention des sauvegardes doivent être adaptées à la criticité de la donnée pour la continuité de la décision commerciale de l'organisation.
- La capacité de restauration doit être vérifiée périodiquement, une sauvegarde non testée n'étant pas considérée comme fiable.

## 4. Journalisation

- Toute action significative sur la plateforme (accès, modification de données, changement de configuration, action d'administration) doit être journalisée.
- La journalisation doit permettre de répondre, à tout moment, aux questions : qui, quoi, quand.
- Les journaux doivent être conservés dans des conditions garantissant leur intégrité et leur non-altération.

## 5. Droits d'accès

- Les droits d'accès sont attribués selon le rôle de l'utilisateur au sein de son organisation (cf. [USER_PERSONAS.md](USER_PERSONAS.md)) et son périmètre de responsabilité (territoire, équipe).
- Toute modification de droits doit être réalisée par un utilisateur habilité (Administrateur) et faire l'objet d'une journalisation.
- Les droits d'accès doivent être révisables à tout moment, sans nécessiter d'intervention en dehors de l'organisation cliente elle-même.

## 6. Authentification

- L'accès à la plateforme requiert une authentification systématique de chaque utilisateur.
- L'organisation cliente doit pouvoir appliquer des exigences renforcées d'authentification, adaptées à la sensibilité de ses données et à son niveau de licence (cf. [LICENSE_SYSTEM.md](LICENSE_SYSTEM.md)).
- Une session inactive doit être automatiquement close après une durée raisonnable, afin de limiter les risques d'accès non autorisé.

## 7. Gestion des données

- Chaque donnée manipulée par la plateforme doit avoir un cycle de vie clair : origine, durée de conservation, conditions de suppression.
- Une organisation cliente doit pouvoir demander la suppression ou l'export de l'intégralité de ses données, conformément à ses obligations réglementaires propres.
- La localisation et les conditions de traitement des données doivent être conformes aux exigences du mode de déploiement choisi (Cloud ou On-Premise, cf. [BUSINESS_MODEL.md](BUSINESS_MODEL.md)).

## 8. Principe transversal

La sécurité n'est jamais une fonctionnalité parmi d'autres : elle est une condition de confiance nécessaire à l'ensemble de la proposition de valeur d'ORBITA AI (cf. [PROJECT_VISION.md](PROJECT_VISION.md)). Tout arbitrage entre sécurité et commodité d'usage doit être documenté explicitement et validé au niveau approprié, jamais décidé implicitement.

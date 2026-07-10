# ORBITA AI — Feature Specification: Audit and History

> Ce document spécifie le comportement fonctionnel complet des modules History et Audit (cf. [architecture/MODULE_ARCHITECTURE.md](../architecture/MODULE_ARCHITECTURE.md) §16-17), couvrant les écrans Historique, Journal et Audit (cf. [ux/SCREEN_STRUCTURE.md](../ux/SCREEN_STRUCTURE.md) §4, §23, §24).

## 1. Objectif

Garantir la traçabilité complète des données et des actions de la plateforme dans le temps, conformément à [docs/SECURITY_PRINCIPLES.md](../docs/SECURITY_PRINCIPLES.md) §2 et §4.

## 2. Historique (des données)

- Conserve l'état successif des entités de l'Organisation (Clients, Produits, Objectifs, Prévisions) au fil des Imports et modifications (cf. [architecture/DATA_FLOW.md](../architecture/DATA_FLOW.md) §7).
- Permet de reconstituer la situation de la performance commerciale à une date ou une période antérieure, et de comparer deux périodes (cf. [ux/SCREEN_STRUCTURE.md](../ux/SCREEN_STRUCTURE.md) §4).
- Une évolution de la structure organisationnelle (territoires, équipes) n'efface jamais l'historique consolidé selon la structure antérieure : les deux restent consultables selon le contexte de la période analysée.

## 3. Journal (activité technique générale)

- Enregistre le déroulement courant de la plateforme pour l'Organisation : démarrage de traitements, achèvement d'Imports, événements internes significatifs (cf. [architecture/LOGGING_STRATEGY.md](../architecture/LOGGING_STRATEGY.md) §2).
- Consultable par les utilisateurs habilités (Administrateur, Contrôle de Gestion), filtrable par module et par niveau de gravité.
- Sert de première référence pour comprendre le fonctionnement courant de la plateforme sans recourir à un incident particulier.

## 4. Audit (actions utilisateurs et administratives)

- Enregistre toute action significative affectant les données ou la configuration : connexion, création, modification, suppression, changement de droits ou de licence (cf. [architecture/LOGGING_STRATEGY.md](../architecture/LOGGING_STRATEGY.md) §3).
- Répond systématiquement aux questions **qui**, **quoi**, **quand**, sans exception.
- Une entrée d'audit, une fois enregistrée, ne peut jamais être modifiée ou supprimée, y compris par un Administrateur (cf. [architecture/LOGGING_STRATEGY.md](../architecture/LOGGING_STRATEGY.md) §3).
- Consultable et filtrable par utilisateur, type d'action et période, réservé aux profils habilités (Administrateur, Contrôle de Gestion).

## 5. Traçabilité transversale

- Toute donnée présentée sur la plateforme (indicateur, analyse, prévision, rapport) reste retraçable jusqu'à son origine : l'Import qui l'a produite, la date de sa consolidation et, le cas échéant, l'utilisateur ayant réalisé une modification manuelle (cf. [docs/SECURITY_PRINCIPLES.md](../docs/SECURITY_PRINCIPLES.md) §2).
- Cette traçabilité est accessible depuis chaque entité (ex. depuis une fiche Client, accès à son historique de modifications) sans nécessiter de consulter séparément le Journal ou l'Audit général.

## 6. Rétention et export

- La durée de conservation de l'Historique, du Journal et de l'Audit est définie selon leur finalité respective (cf. [architecture/LOGGING_STRATEGY.md](../architecture/LOGGING_STRATEGY.md) §7), l'Audit bénéficiant d'une rétention longue cohérente avec les obligations de conformité de l'Organisation.
- Le contenu du Journal et de l'Audit peut être exporté (cf. [EXPORT_ENGINE.md](EXPORT_ENGINE.md)) pour les besoins de conformité ou d'investigation du Contrôle de Gestion.

## 7. Cloisonnement

Historique, Journal et Audit sont strictement cloisonnés par Organisation, au même titre que toute autre donnée (cf. [architecture/DOMAIN_MODEL.md](../architecture/DOMAIN_MODEL.md) §4) : aucune Organisation ne peut consulter la traçabilité d'une autre.

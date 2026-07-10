import type { ReactElement } from "react";
import { ORBITA_AI_DESIGN_SYSTEM_PACKAGE_MARKER } from "@orbita-ai/design-system";
import { ORBITA_AI_SHARED_PACKAGE_MARKER } from "@orbita-ai/shared";

/**
 * Composant racine de l'application de présentation (tech/PROJECT_STRUCTURE.md §2).
 * Vérifie uniquement que l'application se résout correctement vers le Design System
 * et les utilitaires partagés — aucun écran fonctionnel n'est implémenté à ce stade
 * (cf. ux/SCREEN_STRUCTURE.md, planning/DEVELOPMENT_PHASES.md — Phase 5, Dashboard).
 *
 * Squelette architectural uniquement : aucune logique métier, aucune navigation,
 * aucun composant visuel réel.
 */
export function App(): ReactElement {
  return (
    <div data-orbita-ai-skeleton={`${ORBITA_AI_DESIGN_SYSTEM_PACKAGE_MARKER}+${ORBITA_AI_SHARED_PACKAGE_MARKER}`} />
  );
}

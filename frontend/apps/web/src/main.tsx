import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import { App } from "./App";

/**
 * Point d'assemblage exécutable de la Présentation (tech/PROJECT_STRUCTURE.md §2),
 * pendant frontend du OrbitaAI.Host backend (backend/src/Host/Program.cs).
 * Squelette architectural uniquement — aucun écran fonctionnel, aucune navigation.
 *
 * Le choix de l'outillage de bundling et de serveur de développement (non fixé dans
 * tech/TECHNOLOGY_STACK.md ni tech/DEVELOPMENT_ENVIRONMENT.md) reste un point ouvert,
 * à trancher et documenter avant la clôture de la Phase 1 — Fondations techniques
 * (cf. planning/DEVELOPMENT_PHASES.md).
 */
const rootElement = document.getElementById("root");

if (rootElement) {
  createRoot(rootElement).render(
    <StrictMode>
      <App />
    </StrictMode>,
  );
}

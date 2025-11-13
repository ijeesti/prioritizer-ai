# 🧠 Prioritizer-AI: Intelligent Decision Synthesis Engine

## 🎯 Project Overview

Prioritizer-AI demonstrates a highly **resilient, microservice architecture** designed to eliminate friction in enterprise decision-making (e.g., feature prioritization). This system replaces sequential human bottlenecks with a collaborative network of specialized **AI Agents**, ensuring decisions are fast, consistent, and fully auditable. The initial release is built primarily on **C#/.NET**, with a clear path for integrating polyglot services.

#### ✨ Key Architectural Concepts

This system is built upon three core pillars:

* **Specialized AI Agents:** Dedicated agents (Policy, Tech, Product) are fluent in their specific domain rules, ensuring high-quality, reliable analysis.
* **Microservices & Decoupling:** Agents operate independently, communicating via messaging. The framework is inherently **polyglot-ready**, designed to easily swap in services built in other languages (like Python) for specialized tasks.
* **Event-Driven Orchestration:** All communication is handled via **RabbitMQ**, guaranteeing full decoupling, resilience, and complete auditability of every message and decision step.

---

## 🏃 Getting Started

This repository contains multiple C# projects (consumers, publishers, models) within the main solution.

#### Prerequisites

To run this project locally, ensure you have:

1.  **.NET 10 SDK** installed.
2.  A running instance of **RabbitMQ** (easiest via a Docker container).

#### Setup & Execution

1.  **Clone the Repository:**
    ```bash
    git clone https://github.com/ijeesti/prioritizer-ai.git
    ```
2.  **Configuration:** Set up necessary configurations (RabbitMQ connection string, AI service keys, etc.) in the respective project files.
3.  **Run Agents:** Start all individual C# agent projects (Policy, Tech, Product) and the API publisher from the main Visual Studio solution or via `dotnet run`.

---

### Submit Request

Use the provided API project or script to submit a sample request and initiate the event flow:

```json
{
  "proposal": "Add a one-click checkout option to reduce cart abandonment on the e-commerce site.",
  "businessValue": "Expected to increase conversion rate by 15% and reduce friction during checkout.",
  "requestedBy": "E-commerce Product Lead - Alex Martinez",
  "productName": "ShopEase Web"
}

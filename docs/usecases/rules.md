## Use Case Diagram Rules

- **Source of truth**: Mirror concrete behavior from the corresponding handler/service class (e.g., `RegisterHandler`, `LoginHandler`). Re-read the handler before editing the diagram.
- **Participant naming**: Reflect real components. Use `Actor` for end user, UI layers as `boundary`, controllers/handlers as `control`, caches/services accordingly, and include `database DB` whenever persistence is involved.
- **Interaction order**: Keep the diagram chronological. Show async calls with `->` and returns with `-->`. Only include operations the handler actually performs.
- **Failure handling**: Represent each distinct error branch (`alt` blocks) that results in a returned error or alternate response. Omit branches that cannot currently occur.
- **Diagram hygiene**: Keep PlantUML syntax valid, avoid redundant participants, and ensure each activation/deactivation (`activate`/`deactivate`) matches the actual flow.

## Reference resources

- **Docs**: `https://plantuml.com/sequence-diagram`

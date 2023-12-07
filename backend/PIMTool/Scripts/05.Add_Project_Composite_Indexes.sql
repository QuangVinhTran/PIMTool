CREATE INDEX idx_regular_search
ON [Project] (ProjectNumber, Name, Customer)

CREATE INDEX idx_regular_search_with_status
ON [Project] (ProjectNumber, Name, Customer, Status)
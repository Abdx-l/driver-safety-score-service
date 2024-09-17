CREATE TABLE IF NOT EXISTS trips(id bigserial PRIMARY KEY,driver_id text NOT NULL,occurred timestamp NOT NULL,score numeric CHECK(score BETWEEN 0 AND 100));

USE asf_cms;
UPDATE publication SET news_ttl=(news_ttl - 1) WHERE news_ttl > 0 AND news_pin=false;
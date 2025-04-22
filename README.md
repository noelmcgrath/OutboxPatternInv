# OutboxPatternInv
OutboxPatternInv

# sql
docker build  -t  myappsql  .
docker run -t  myappsql -d -p 1433:1433
docker run -t -d -p 1433:1433 --name myappsql myappsql 

# rabbit
cd D:\noel\OutboxPatternInv\rabbitmq
docker build  -t  myrabbit .
docker run -t -d -p 5672:5672 -p 15672:15672 --name myrabbit myrabbit 

BfTtrMgh7y6a



rabbitmqadmin --username=ccsappuser --password=BfTtrMgh7y6a list queues name
rabbitmqadmin --username=ccsappuser --password=BfTtrMgh7y6a list queues name --quiet

for q in $(rabbitmqadmin --username=ccsappuser --password=BfTtrMgh7y6a list queues name --quiet); do
    rabbitmqadmin --username=ccsappuser --password=BfTtrMgh7y6a purge queue name="$q"
done



Publishing where rabbit gone, results in duplicate
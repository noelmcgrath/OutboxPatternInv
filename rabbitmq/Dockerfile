FROM rabbitmq:management-alpine

ADD rabbitmq.conf /etc/rabbitmq/
ADD definitions.json /etc/rabbitmq/
ADD thehost.rabbit.at_key.pem /etc/ssl/certs/
ADD thehost.rabbit.at.pem /etc/ssl/certs/

RUN chown rabbitmq:rabbitmq /etc/rabbitmq/rabbitmq.conf /etc/rabbitmq/definitions.json 

CMD ["rabbitmq-server"]
